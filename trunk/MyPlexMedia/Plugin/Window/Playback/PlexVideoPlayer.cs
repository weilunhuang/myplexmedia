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
using MediaPortal.Player;
using MyPlexMedia.Plugin.Window.Dialogs;
using PlexMediaCenter.Plex.Data.Types;

namespace MyPlexMedia.Plugin.Window.Playback {
    internal static class PlexVideoPlayer {
        private static readonly Timer BufferCheckTimer;

        static PlexVideoPlayer() {
            BufferCheckTimer = new Timer {Interval = 5000};
            BufferCheckTimer.Tick += _bufferCheckTimer_Tick;

            g_Player.PlayBackStarted += g_Player_PlayBackStarted;
            g_Player.PlayBackStopped += g_Player_PlayBackStopped;
            Buffering.OnPreBufferingProgress += Buffering_OnPreBufferingProgress;
            Buffering.OnPlayPreBufferedMedia += Buffering_OnPlayBufferedMedia;
            Buffering.OnBufferingProgress += Buffering_OnBufferingProgress;
        }

        private static void g_Player_PlayBackStopped(g_Player.MediaType type, int stoptime, string filename) {
            BufferCheckTimer.Stop();
            if (Buffering.IsBuffering) {
                //Damn, we must have been out of buffered stuff there...
                //TODO: Gently ask user to lower his/her expectations a litte and crank quality down/prebuffer up!!!
                Buffering.StopBuffering();
            }
        }

        private static void g_Player_PlayBackStarted(g_Player.MediaType type, string filename) {
            BufferCheckTimer.Start();
        }

        private static void _bufferCheckTimer_Tick(object sender, EventArgs e) {
            if (Buffering.IsBuffering) {
                if (GetRemainingBufferPercentage() <= 5) {
                    //we're running out of oxygen here... pause playback to be safe!
                    if (g_Player.Playing) {
                        g_Player.Pause();
                    }
                } else {
                    //back in business... unpause, UNPAUSE!
                    if (g_Player.Paused) {
                        g_Player.Pause();
                    }
                }
            }
        }

        private static int GetRemainingBufferPercentage() {
            try {
                return (int) (100 - (g_Player.CurrentPosition*100/g_Player.Duration));
            } catch {
                return 0;
            }
        }

        private static void Buffering_OnPreBufferingProgress(int currentProgress, string infoText) {
            CommonDialogs.ShowProgressDialog(currentProgress, string.Empty, infoText);
        }

        public static void PlayBackMedia(Uri itemPath, MediaContainerVideo video) {
            CommonDialogs.ShowProgressDialog(0, "Buffering...", video.title);
            Buffering.BufferMedia(itemPath, video);
        }

        private static void Buffering_OnBufferingProgress(int currentProgress, string infoText) {
            if (g_Player.Paused) {
                CommonDialogs.ShowProgressDialog(currentProgress, "", infoText);
            } else {
                CommonDialogs.HideProgressDialog();
            }
        }

        private static void Buffering_OnPlayBufferedMedia(string localBufferPath) {
            CommonDialogs.HideProgressDialog();
            g_Player.Init();
            g_Player.SetVideoWindow();
            g_Player.PlayVideoStream(localBufferPath);

            //if (Settings.PLUGIN_WINDOW_ID == GUIWindowManager.ActiveWindow) {
            //    GUIWindowManager.ActivateWindow((int)GUIWindow.Window.WINDOW_TVFULLSCREEN);
            //}
        }
    }
}