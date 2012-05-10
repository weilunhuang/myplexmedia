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
using System.Diagnostics;
using System.IO;
using System.Net;
using MediaPortal.GUI.Library;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Util;
using System.Threading;
using MyPlexMedia.Plugin.Config;

namespace MyPlexMedia.Plugin.Window.Playback {

       internal static class Buffering {      

        #region Delegates

        public delegate void OnBufferingProgressEventHandler(int currentProgress, BufferJob currentJobInfo);

        public delegate void OnPlayPreBufferedMediaEventHandler(string localBufferPath, BufferJob currentJobInfo);

        #endregion

        private const PlexQualities DefaultQuality = PlexQualities._1_320kbps_240p;
        private static readonly BackgroundWorker MediaBufferer;


        static Buffering() {
            //logger.Info(" started...");       
            DeleteBufferFile();
            MediaBufferer = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
            MediaBufferer.RunWorkerCompleted += _mediaBufferer_RunWorkerCompleted;
            MediaBufferer.DoWork += MediaBufferer_DoWork;
            MediaBufferer.ProgressChanged += MediaBufferer_ProgressChanged;
        }

        public static bool IsBuffering { get { return MediaBufferer.IsBusy; } }
        public static bool IsPreBuffering { get; set; }
        public static BufferJob CurrentJob { get; private set; }
        private static int CurrentSpeedAverage { get; set; }

        public static event OnBufferingProgressEventHandler OnBufferingProgress;
        public static event OnPlayPreBufferedMediaEventHandler OnPlayPreBufferedMedia;

        public static void StopBuffering() {
            if (MediaBufferer.IsBusy) {
                //logger.Info("Request Buffering Cancellation");
                MediaBufferer.CancelAsync();
                Transcoding.StopTranscoding(CurrentJob.ServerPath);
            }
        }

        internal static void BufferMedia(Uri plexUriPath, MediaContainerVideo video, PlexQualities quality = DefaultQuality, long offset = 0, bool is3G = false) {
            StopBuffering();
            IsPreBuffering = true;
            CurrentJob = new BufferJob {
                ServerPath = plexUriPath,
                Video = video,
                Quality = quality,
                Is3G = is3G,
                Offset = offset,
                PreBufferSize = (int)quality
            };
            MediaBufferer.RunWorkerAsync(CurrentJob);
        }

        private static void DeleteBufferFile() {
            if (File.Exists(Settings.PLEX_BUFFER_FILE)) {
                File.Delete(Settings.PLEX_BUFFER_FILE);
            }
        }

        private static void MediaBufferer_DoWork(object sender, DoWorkEventArgs e) {
            //logger.Info("BackGroundWorker - Buffering...");

            if (!(e.Argument is BufferJob)) return;
            BufferJob currentJob = (BufferJob)e.Argument;
            currentJob.SegmentsBuffered = 0;
            MediaBufferer.ReportProgress(0, currentJob);
            List<string> segments = Transcoding.GetVideoSegmentedPlayList(currentJob.ServerPath, currentJob.Video,
                                                                          currentJob.Offset,
                                                                          (int)currentJob.Quality,
                                                                          currentJob.Is3G);           
            currentJob.SegmentsCount = segments.Count;
            currentJob.SpeedIssues = false;

            DeleteBufferFile();
            using (
                FileStream bufferedMedia = new FileStream(Settings.PLEX_BUFFER_FILE, FileMode.Create, FileAccess.Write,
                                                          FileShare.Read)
                ) {
                foreach (string segment in segments) {
                    if (MediaBufferer.CancellationPending) {
                        //logger.Info("BackGroundWorker - CancellationPending detected - cancelling asynchronous buffering...");
                        e.Cancel = true;
                        IsPreBuffering = false;
                        bufferedMedia.Flush();
                        bufferedMedia.Close();
                        break;
                    }
                    using (WebClient segmentFetcher = new WebClient()) {
                        Stopwatch timer = new Stopwatch();
                        timer.Start();
                        byte[] data = segmentFetcher.DownloadData(segment);
                        timer.Stop();
                        currentJob.SpeedIssues = CheckSpeedQuality(data.Length, timer.Elapsed.TotalSeconds,
                                                                   currentJob.Quality);
                        bufferedMedia.Write(data, 0, data.Length);
                        bufferedMedia.Flush();
                    }
                    MediaBufferer.ReportProgress((currentJob.SegmentsBuffered * 100 / currentJob.SegmentsCount),
                                                 currentJob);
                    if (++currentJob.SegmentsBuffered == currentJob.PreBufferSize) {
                        IsPreBuffering = false;
                        OnPlayPreBufferedMedia(Settings.PLEX_BUFFER_FILE, currentJob);
                    }
                }
            }

        }

        private static bool CheckSpeedQuality(double dataLength, double totalSeconds, PlexQualities plexQuality) {
            if (totalSeconds < 1) {
                totalSeconds = 1;
            }
            int currentSpeed = (int)(dataLength * 8 / totalSeconds); //bps

            if (CurrentSpeedAverage == 0) {
                CurrentSpeedAverage = (currentSpeed / 1024); //kbps
            } else {
                CurrentSpeedAverage += (currentSpeed / 1024);
                CurrentSpeedAverage /= 2;
            }
            Log.Debug("Current download speed: {0, 15} kbps / selected quality: {1}", new object[] { CurrentSpeedAverage, plexQuality });
            switch (plexQuality) {
                case PlexQualities._1_320kbps_240p:
                    break;
                case PlexQualities._2_720kbps_320p:
                    break;
                case PlexQualities._3_1500kbps_480p:
                    break;
                case PlexQualities._4_2000kbps_720p:
                    break;
                case PlexQualities._5_3000kbps_720p:
                    break;
                case PlexQualities._6_4000kbps_720p:
                    break;
                case PlexQualities._7_8000kbps_1080p:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("plexQuality");
            }
            return false;
        }

        private static void MediaBufferer_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            OnBufferingProgress(e.ProgressPercentage, (BufferJob)e.UserState);
        }

        private static void _mediaBufferer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Cancelled) {
                //logger.Warn("BackGroundWorker -Buffering completed and was cancelled");
            } else if (e.Error != null) {
                //logger.ErrorException("BackGroundWorker -Buffering completed with Exception", e.Error);
            } else {
                //logger.Info("BackGroundWorker -Buffering completed and was successful");
            }

        }
    }

    public class BufferJob {
        public Uri ServerPath { get; set; }
        public MediaContainerVideo Video { get; set; }
        public long Offset { get; set; }

        public bool Is3G { get; set; }
        public bool SpeedIssues { get; set; }
        public PlexQualities Quality { get; set; }

        public int PreBufferSize { get; set; }
        public int SegmentsBuffered { get; set; }
        public int SegmentsCount { get; set; }

        public double BufferingProgress {
            get {
                if (SegmentsCount == 0) {
                    return 0;
                }
                return (((VideoDuration / SegmentsCount) * SegmentsBuffered) * 100) / VideoDuration;
            }
        }

        public double PreBufferingProgress {
            get {
                if (SegmentsCount == 0) {
                    return 0;
                }
                return (SegmentsBuffered * 100) / PreBufferSize;
            }
        }

        public double VideoDuration {
            get { return int.Parse(Video.duration) / 1000d; }
        }
    }
}