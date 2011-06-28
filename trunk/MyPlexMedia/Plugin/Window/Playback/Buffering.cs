#region #region Copyright (C) 2005-2011 Team MediaPortal

// 
// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.
// 

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Util;

namespace MyPlexMedia.Plugin.Window.Playback {
    internal static class Buffering {
        #region Delegates

        public delegate void OnBufferingProgressEventHandler(int currentProgress, string infoText);

        public delegate void OnPlayPreBufferedMediaEventHandler(string localBufferPath);

        #endregion

        private const string BufferFile = @"D:\buffer.ts";
        private static int DefaultBuffer = 1;
        private const int DefaultQuality = 3;
        private static readonly BackgroundWorker MediaBufferer;

        static Buffering() {
            //logger.Info(" started...");
            DeleteBufferFile();
            MediaBufferer = new BackgroundWorker {WorkerSupportsCancellation = true, WorkerReportsProgress = true};
            MediaBufferer.RunWorkerCompleted += _mediaBufferer_RunWorkerCompleted;
            MediaBufferer.DoWork += MediaBufferer_DoWork;
            MediaBufferer.ProgressChanged += new ProgressChangedEventHandler(MediaBufferer_ProgressChanged);
        }       

        public static bool IsBuffering { get; set; }
        public static event OnBufferingProgressEventHandler OnBufferingProgress;
        public static event OnPlayPreBufferedMediaEventHandler OnPlayPreBufferedMedia;

        public static void StopBuffering() {
            if (MediaBufferer.IsBusy) {
                //logger.Info("Request Buffering Cancellation");
                MediaBufferer.CancelAsync();
            }
        }

        internal static void BufferMedia(Uri plexUriPath, MediaContainerVideo video, long offset = 0,
                                         int quality = DefaultQuality, bool is3G = false) {
            StopBuffering();
            DefaultBuffer = quality;
            MediaBufferer.RunWorkerAsync(Transcoding.GetVideoSegmentedPlayList(plexUriPath, video, offset, quality, is3G));
        }

        private static void DeleteBufferFile() {
            if (File.Exists(BufferFile)) {
                File.Delete(BufferFile);
            }
        }

        private static void MediaBufferer_DoWork(object sender, DoWorkEventArgs e) {
            //logger.Info("BackGroundWorker - Buffering...");
            if (!(e.Argument is List<string>)) return;
            List<string> segments = (List<string>) e.Argument;
            IsBuffering = true;
            using (
                FileStream bufferedMedia = new FileStream(BufferFile, FileMode.Create, FileAccess.Write, FileShare.Read)
                ) {
                int bufferedSegments = 0;
                foreach (string segment in segments) {
                    if (MediaBufferer.CancellationPending) {
                        //logger.Info("BackGroundWorker - CancellationPending detected - cancelling asynchronous buffering...");
                        e.Cancel = true;
                        bufferedMedia.Flush();
                        bufferedMedia.Close();
                        return;
                    }
                    try {
                        using (WebClient segmentFetcher = new WebClient()) {
                            byte[] data = segmentFetcher.DownloadData(segment);
                            bufferedMedia.Write(data, 0, data.Length);
                            bufferedMedia.Flush();
                        }
                    } catch {
                        throw;
                    }                    
                    MediaBufferer.ReportProgress((int)(bufferedSegments * 100 / segments.Count), String.Format("Segments: {0, -3} / {1, 3}", bufferedSegments, segments.Count));
                    if (++bufferedSegments == DefaultBuffer) {
                        OnPlayPreBufferedMedia(BufferFile);
                    }
                }
            }
        }

        static void MediaBufferer_ProgressChanged(object sender, ProgressChangedEventArgs e) {
           OnBufferingProgress(e.ProgressPercentage, e.UserState.ToString());
        }

        private static void _mediaBufferer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Cancelled) {
                //logger.Warn("BackGroundWorker -Buffering completed and was cancelled");
            } else if (e.Error != null) {
                //logger.ErrorException("BackGroundWorker -Buffering completed with Exception", e.Error);
            } else {
                //logger.Info("BackGroundWorker -Buffering completed and was successful");
            }
            IsBuffering = false;
            DeleteBufferFile();
        }
    }

    public static class StreamExtensions {
        private const int DEFAULT_BUFFER_SIZE = short.MaxValue; // +32767

        public static void CopyTo(this Stream input, Stream output) {
            input.CopyTo(output, DEFAULT_BUFFER_SIZE);
            return;
        }

        public static void CopyTo(this Stream input, Stream output, int bufferSize) {
            if (!input.CanRead) throw new InvalidOperationException("input must be open for reading");
            if (!output.CanWrite) throw new InvalidOperationException("output must be open for writing");

            byte[][] buf = {new byte[bufferSize], new byte[bufferSize]};
            int[] bufl = {0, 0};
            int bufno = 0;
            IAsyncResult read = input.BeginRead(buf[bufno], 0, buf[bufno].Length, null, null);
            IAsyncResult write = null;

            while (true) {
                // wait for the read operation to complete
                read.AsyncWaitHandle.WaitOne();
                bufl[bufno] = input.EndRead(read);

                // if zero bytes read, the copy is complete
                if (bufl[bufno] == 0) {
                    break;
                }

                // wait for the in-flight write operation, if one exists, to complete
                // the only time one won't exist is after the very first read operation completes
                if (write != null) {
                    write.AsyncWaitHandle.WaitOne();
                    output.EndWrite(write);
                }

                // start the new write operation
                write = output.BeginWrite(buf[bufno], 0, bufl[bufno], null, null);

                // toggle the current, in-use buffer
                // and start the read operation on the new buffer
                bufno = (bufno == 0 ? 1 : 0);
                read = input.BeginRead(buf[bufno], 0, buf[bufno].Length, null, null);
            }

            // wait for the final in-flight write operation, if one exists, to complete
            // the only time one won't exist is if the input stream is empty.
            if (write != null) {
                write.AsyncWaitHandle.WaitOne();
                output.EndWrite(write);
            }

            output.Flush();

            // return to the caller ;
            return;
        }
    }
}