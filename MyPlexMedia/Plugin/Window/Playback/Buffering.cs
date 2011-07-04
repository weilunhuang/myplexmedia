﻿#region #region Copyright (C) 2005-2011 Team MediaPortal

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
    public enum PlexQualities {
        _1_320kbps240p = 3,
        _2_720kbps320p,
        _3_1500kbps480p,
        _4_2000kbps720p,
        _5_3000kbps720p,
        _6_4000kbps720p,
        _7_8000kbps1080p
    }

    internal static class Buffering {
        #region Delegates

        public delegate void OnBufferingProgressEventHandler(int currentProgress, BufferJob currentJobInfo);

        public delegate void OnPlayPreBufferedMediaEventHandler(string localBufferPath, BufferJob currentJobInfo);

        #endregion

        private const string BufferFile = @"D:\buffer.ts";
        private const PlexQualities DefaultQuality = PlexQualities._1_320kbps240p;
        private static readonly BackgroundWorker MediaBufferer;

        static Buffering() {
            //logger.Info(" started...");       
            DeleteBufferFile();
            MediaBufferer = new BackgroundWorker {WorkerSupportsCancellation = true, WorkerReportsProgress = true};
            MediaBufferer.RunWorkerCompleted += _mediaBufferer_RunWorkerCompleted;
            MediaBufferer.DoWork += MediaBufferer_DoWork;
            MediaBufferer.ProgressChanged += MediaBufferer_ProgressChanged;
        }

        public static long Offset { get; set; }
        public static PlexQualities Quality { get; set; }
        public static int Buffer { get; set; }
        public static bool Is3G { get; set; }

        public static bool IsBuffering { get; set; }
        public static BufferJob CurrentJob { get; private set; }

        public static event OnBufferingProgressEventHandler OnBufferingProgress;
        public static event OnPlayPreBufferedMediaEventHandler OnPlayPreBufferedMedia;

        public static void StopBuffering() {
            if (MediaBufferer.IsBusy) {
                //logger.Info("Request Buffering Cancellation");
                MediaBufferer.CancelAsync();
            }
        }

        internal static BufferJob BufferMedia(Uri plexUriPath, MediaContainerVideo video, long offset = 0,
                                         PlexQualities quality = DefaultQuality, bool is3G = false) {
            StopBuffering();
            Offset = offset;
            Is3G = is3G;
            Quality = quality;
            Buffer = (int) Quality;
            BufferJob nextJob = new BufferJob { ServerPath = plexUriPath, Video = video };
            MediaBufferer.RunWorkerAsync(nextJob);
            return nextJob;
        }

        private static void DeleteBufferFile() {
            if (File.Exists(BufferFile)) {
                try {
                    File.Delete(BufferFile);
                }catch{}
            }
        }

        private static void MediaBufferer_DoWork(object sender, DoWorkEventArgs e) {
            //logger.Info("BackGroundWorker - Buffering...");
            if (!(e.Argument is BufferJob)) return;
            CurrentJob = (BufferJob) e.Argument;
            MediaBufferer.ReportProgress(0, CurrentJob);
            List<string> segments = Transcoding.GetVideoSegmentedPlayList(CurrentJob.ServerPath, CurrentJob.Video,
                                                                          Offset, (int) Quality, Is3G);
            CurrentJob.SegmentsBuffered = 0;
            CurrentJob.SegmentsCount = segments.Count;
            IsBuffering = true;
            //Path.ChangeExtension(BufferFile, Is3G ? "ts" : "ts");
            DeleteBufferFile();

            using (
                FileStream bufferedMedia = new FileStream(BufferFile, FileMode.Create, FileAccess.Write, FileShare.Read)
                ) {
                  foreach (string segment in segments) {
                    if (MediaBufferer.CancellationPending) {
                        //logger.Info("BackGroundWorker - CancellationPending detected - cancelling asynchronous buffering...");
                        e.Cancel = true;
                        bufferedMedia.Flush();
                        bufferedMedia.Close();
                        return;
                    }
                    using (WebClient segmentFetcher = new WebClient()) {
                        byte[] data = segmentFetcher.DownloadData(segment);
                        bufferedMedia.Write(data, 0, data.Length);
                        bufferedMedia.Flush();                        
                    }
                    //HttpWebRequest request = (HttpWebRequest) WebRequest.Create(segment);
                    //request.Timeout = 5000; // milliseconds, adjust as needed
                    //request.ReadWriteTimeout = 10000; // milliseconds, adjust as needed
                    //using (WebResponse response = request.GetResponse()) {
                    //    using (Stream responseStream = response.GetResponseStream()) {
                    //        // Process the stream
                    //        try {
                    //            byte[] data = new byte[1024];
                    //            int count;
                    //            do {
                    //                count = responseStream.Read(data, 0, data.Length);
                    //                bufferedMedia.Write(data, 0, data.Length);
                    //                bufferedMedia.Flush();
                    //            } while (count > 0 || MediaBufferer.CancellationPending);
                    //        } finally {
                    //            responseStream.Close();
                    //            response.Close();
                    //        }
                    //    }
                    //}
                    MediaBufferer.ReportProgress((CurrentJob.SegmentsBuffered*100/CurrentJob.SegmentsCount), CurrentJob);
                    if (++CurrentJob.SegmentsBuffered == Buffer) {
                        OnPlayPreBufferedMedia(BufferFile, CurrentJob);
                    }
                }
            }
        }

        private static void MediaBufferer_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            OnBufferingProgress(e.ProgressPercentage, (BufferJob) e.UserState);
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
        }
    }

    public class BufferJob {
        public Uri ServerPath { get; set; }
        public MediaContainerVideo Video { get; set; }
        public int SegmentsBuffered { get; set; }
        public int SegmentsCount { get; set; }

        public double BufferingProgress {
            get {
                if (SegmentsCount == 0) {
                    return 0;
                }
                return (((VideoDuration/SegmentsCount)*SegmentsBuffered)*100)/VideoDuration;
            }
        }

        public double VideoDuration {
            get { return int.Parse(Video.duration)/1000d; }
        }
    }
}