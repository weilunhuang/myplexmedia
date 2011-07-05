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
using System.Windows.Forms;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
using MyPlexMedia.Plugin.Config;
using MyPlexMedia.Plugin.Window.Dialogs;
using PlexMediaCenter.Plex.Data.Types;

namespace MyPlexMedia.Plugin.Window.Playback {
    internal static class PlexVideoPlayer {
       
        private static readonly Timer BufferCheckTimer;

        static PlexVideoPlayer() {
            BufferCheckTimer = new Timer {Interval = 1000};
            BufferCheckTimer.Tick += _bufferCheckTimer_Tick;
            
            Buffering.OnPlayPreBufferedMedia += Buffering_OnPlayBufferedMedia;
            Buffering.OnBufferingProgress += Buffering_OnBufferingProgress;
            CommonDialogs.OnProgressCancelled += CommonDialogs_OnProgressCancelled;
        }

        private static bool BufferingPause { get; set; }

        public static void PlayBackMedia(Uri itemPath, MediaContainerVideo video) {
            CommonDialogs.ShowWaitCursor();
            Buffering.BufferMedia(itemPath, video, 0, PlexQualities._3_1500kbps480p, false);
            BufferCheckTimer.Start();
            while(Buffering.IsBuffering && Buffering.IsPreBuffering) {
                GUIWindowManager.Process();
            }
            CommonDialogs.HideWaitCursor();
        }

        private static void Buffering_OnPlayBufferedMedia(string localBufferPath, BufferJob bufferJob) {
            BufferingPause = false;
            PlayPlayerMainThread(localBufferPath, bufferJob.Video.title);
            new System.Threading.Thread(delegate(object o) {
                System.Threading.Thread.Sleep(1000);
                SetGuiProperties(o);
            }).Start(bufferJob.Video);
        }

        private static void Buffering_OnBufferingProgress(int currentProgress, BufferJob bufferJob) {
            GUIPropertyManager.SetProperty("#TV.Record.percent2", bufferJob.BufferingProgress.ToString());
            GUIPropertyManager.SetProperty("#TV.Record.percent3", "100");
            if (Buffering.IsPreBuffering || g_Player.Paused) {
                //CommonDialogs.ShowBufferingProgressDialog("Buffering...", bufferJob.Video.title, bufferJob.SegmentsBuffered.ToString(), bufferJob.SegmentsCount.ToString(), "Progress:", ((int)(g_Player.CurrentPosition * 100 / Buffering.CurrentJob.VideoDuration)), (int)Buffering.CurrentJob.BufferingProgress);
            }
        }

        private static void CommonDialogs_OnProgressCancelled() {
            StopPlayerMainThread();
        }

        private static int GetRemainingBufferedPlayTimePercentage() {
            try {
                return (int)(100 - (g_Player.CurrentPosition * 100 / g_Player.Duration));
            } catch {
                return 0;
            }
        }
        
        private static void _bufferCheckTimer_Tick(object sender, EventArgs e) {
            if (Buffering.IsBuffering) {
                if (GetRemainingBufferedPlayTimePercentage() < 10) {
                    //we're running out of oxygen here... pause playback to be safe!
                    if (!g_Player.Paused && !BufferingPause) {
                        BufferingPause = true;
                        PausePlayerMainThread();
                    }
                } else {
                    //back in business... unpause, UNPAUSE!
                    if (g_Player.Paused && BufferingPause) {
                        BufferingPause = false;
                        PausePlayerMainThread();
                    }
                }
            }

            GUIPropertyManager.SetProperty("#duration", Buffering.CurrentJob.VideoDuration.ToString());
            GUIPropertyManager.SetProperty("#TV.View.Percentage",
                                           ((int)(g_Player.CurrentPosition * 100 / Buffering.CurrentJob.VideoDuration)).ToString());
            GUIPropertyManager.SetProperty("#TV.Record.percent1",
                                           ((int)(g_Player.CurrentPosition * 100 / Buffering.CurrentJob.VideoDuration)).ToString());
        }
        
        private static void SetGuiProperties(object video) {
            MediaContainerVideo nowPlaying = video as MediaContainerVideo;
            GUIPropertyManager.SetProperty("#Play.Current.Title", nowPlaying.title);
            GUIPropertyManager.SetProperty("#Play.Current.File", nowPlaying.Media[0].Part[0].file);
            GUIPropertyManager.SetProperty("#Play.Current.Thumb", Settings.PLEX_ICON_DEFAULT);
            GUIPropertyManager.SetProperty("#Play.Current.Plot", nowPlaying.summary);
            GUIPropertyManager.SetProperty("#Play.Current.PlotOutline", nowPlaying.tagline);
            GUIPropertyManager.SetProperty("#Play.Current.Rating", nowPlaying.rating);
            GUIPropertyManager.SetProperty("#Play.Current.MPAARating", nowPlaying.contentRating);
            GUIPropertyManager.SetProperty("#Play.Current.Year", nowPlaying.year);
            GUIPropertyManager.SetProperty("#Play.Current.Runtime", nowPlaying.duration);
            GUIPropertyManager.SetProperty("#Play.Current.AspectRatio", nowPlaying.Media[0].aspectRatio);
            GUIPropertyManager.SetProperty("#Play.Current.VideoResolution", nowPlaying.Media[0].videoResolution);
            GUIPropertyManager.SetProperty("#Play.Current.VideoCodec.Texture", nowPlaying.Media[0].videoCodec);
            GUIPropertyManager.SetProperty("#Play.Current.AudioCodec.Texture", nowPlaying.Media[0].audioCodec);
            GUIPropertyManager.SetProperty("#Play.Current.AudioChannels", nowPlaying.Media[0].audioChannels);
        }



        private delegate void StopPlayerMainThreadDelegate();

        private static void StopPlayerMainThread() {
            
            //call g_player.stop only on main thread.
            if (GUIGraphicsContext.form.InvokeRequired) {
                StopPlayerMainThreadDelegate d = new StopPlayerMainThreadDelegate(StopPlayerMainThread);
                GUIGraphicsContext.form.Invoke(d);
                return;
            }            
            g_Player.Stop();
        }

        private delegate void PausePlayerMainThreadDelegate();

        private static void PausePlayerMainThread() {
            //call g_player.stop only on main thread.
            if (GUIGraphicsContext.form.InvokeRequired) {
                PausePlayerMainThreadDelegate d = new PausePlayerMainThreadDelegate(PausePlayerMainThread);
                GUIGraphicsContext.form.Invoke(d);
                return;
            }
            g_Player.Pause();
        }

        private delegate void PlayPlayerMainThreadDelegate(string file, string title);

        private static void PlayPlayerMainThread(string file, string title) {
            //call g_player.stop only on main thread.
            if (GUIGraphicsContext.form.InvokeRequired) {
                PlayPlayerMainThreadDelegate d = new PlayPlayerMainThreadDelegate(PlayPlayerMainThread);
                GUIGraphicsContext.form.Invoke(d,new object[]{file,title});
                return;
            }
            g_Player.PlayVideoStream(file, title);
        }

    }
}