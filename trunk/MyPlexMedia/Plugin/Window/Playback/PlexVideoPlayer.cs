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
using MediaPortal.Player;
using MyPlexMedia.Plugin.Window.Dialogs;
using PlexMediaCenter.Plex.Data.Types;
using System.Timers;
using MediaPortal.GUI.Library;
using MyPlexMedia.Plugin.Config;

namespace MyPlexMedia.Plugin.Window.Playback {
    internal static class PlexVideoPlayer {
        public delegate void OnPlexVideoPlayBackEventHandler(MediaContainerVideo currentVideo);
        public static event OnPlexVideoPlayBackEventHandler OnPlexVideoPlayBack;

        private static readonly Timer BufferCheckTimer;

        static PlexVideoPlayer() {
            BufferCheckTimer = new Timer { Interval = 1000 };
            BufferCheckTimer.Elapsed += _bufferCheckTimer_Tick;

            g_Player.PlayBackStarted += g_Player_PlayBackStarted;
            g_Player.PlayBackStopped += g_Player_PlayBackStopped;
            g_Player.PlayBackEnded += g_Player_PlayBackEnded;
            Buffering.OnPlayPreBufferedMedia += Buffering_OnPlayBufferedMedia;
            Buffering.OnBufferingProgress += Buffering_OnBufferingProgress;
            CommonDialogs.OnProgressCancelled += CommonDialogs_OnProgressCancelled;
        }

        static void g_Player_PlayBackStarted(g_Player.MediaType type, string filename) {
            CommonDialogs.HideProgressDialog();
        }

      

        static void CommonDialogs_OnProgressCancelled() {
            Buffering.StopBuffering();
        }

       

        static void g_Player_PlayBackEnded(g_Player.MediaType type, string filename) {
            g_Player_PlayBackStopped(type, (int)g_Player.CurrentPosition, filename);
        }

        private static void g_Player_PlayBackStopped(g_Player.MediaType type, int stoptime, string filename) {           
            BufferCheckTimer.Stop();
            if (Buffering.IsBuffering) {
                //Damn, we must have been out of buffered stuff there...
                //TODO: Gently ask user to lower his/her expectations a litte and crank quality down/prebuffer up!!!
                Buffering.StopBuffering();
            }
        }

        private static bool BufferingPause { get; set; }

        private static void _bufferCheckTimer_Tick(object sender, EventArgs e) {
            if (Buffering.IsBuffering) {
                if (GetRemainingBufferPercentage() < 10) {
                    //we're running out of oxygen here... pause playback to be safe!
                    g_Player.Process();
                    if (!g_Player.Paused && !BufferingPause) {
                        BufferingPause = true;
                        g_Player.Pause();

                    }
                } else {
                    //back in business... unpause, UNPAUSE!
                    if (g_Player.Paused && BufferingPause) {
                        BufferingPause = false;
                        CommonDialogs.HideProgressDialog();
                        g_Player.Pause();
                    }
                }
            }
        }

        private static int GetRemainingBufferPercentage() {
            try {
                return (int)(100 - (g_Player.CurrentPosition * 100 / g_Player.Duration));
            } catch {
                return 0;
            }
        }


        public static void PlayBackMedia(Uri itemPath, MediaContainerVideo video) {
            Buffering.BufferMedia(itemPath, video, 0, PlexQualities._1_320kbps240p, false);
            CommonDialogs.ShowProgressDialog(0, "Buffering...", video.title, true);
        }

        private static void Buffering_OnBufferingProgress(int currentProgress, Buffering.BufferJob bufferJob) {
            if (g_Player.Paused) {
                CommonDialogs.ShowProgressDialog(currentProgress, "Buffering...", bufferJob.Video.title, false);
                GUIWindowManager.Process();
            }
        }
                
        private static void Buffering_OnPlayBufferedMedia(string localBufferPath, Buffering.BufferJob bufferJob) {
            BufferingPause = false;
            g_Player.PlayVideoStream(localBufferPath, bufferJob.Video.title);   
            g_Player.ShowFullScreenWindow();            
            BufferCheckTimer.Start();
            new System.Threading.Thread(delegate(object o) {
                System.Threading.Thread.Sleep(2000);
                SetGuiProperties(o);
            }).Start(bufferJob.Video);           
        }

        private static void SetGuiProperties(object video) {
            var nowPlaying = video as MediaContainerVideo;
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
    }
}