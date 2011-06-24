﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.Player;
using MediaPortal.GUI.Library;
using Vlc.DotNet.Forms;
using Vlc.DotNet.Core;
using System.Drawing;
using System.IO;
using Vlc.DotNet.Core.Medias;
using MyPlexMedia.Plugin.Config;
using Vlc.DotNet.Core.Interops.Signatures.LibVlc.Media;

namespace MyPlexMedia.Plugin.Player {

    public class MyPlexMediaPlayer : IPlayer, IPlayerFactory{

        enum PlayState { Init, Playing, Paused, Ended };

        VlcControl vlcCtrl;
        PathMedia media;
        PlayState playState;
       
        string url;
        bool bufferingDone;
        float playPosition;

        public bool PlaySegmentedVideoStream(Uri m3u8PlayListUrl) {
            IPlayerFactory savedFactory = g_Player.Factory;
            g_Player.Factory = this as IPlayerFactory;
            bool playing = g_Player.Play(m3u8PlayListUrl.AbsoluteUri , g_Player.MediaType.Video);
            g_Player.Factory = savedFactory;
            return playing;
        }

        public override bool IsExternal {
            get {
                return false;
            }
        }

       public override bool Play(string strFile) {            
            url = strFile;

            VlcContext.StartupOptions.IgnoreConfig = true;
            VlcContext.StartupOptions.LogOptions.LogInFile = true;
            VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Debug;
            VlcContext.StartupOptions.AddOption("--no-video-title-show");
            VlcContext.StartupOptions.AddOption("--http-caching=5000");
            VlcContext.StartupOptions.LogOptions.LogInFilePath = Path.Combine(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Log), "vlc-onlinevideos.log");
            
            if (IsInstalled) {
                VlcContext.LibVlcDllsPath = vlcPath;
                VlcContext.LibVlcPluginsPath = Path.Combine(vlcPath, "plugins");
            }

            vlcCtrl = new VlcControl();
            GUIGraphicsContext.form.Controls.Add(vlcCtrl);
            vlcCtrl.Enabled = false;

            vlcCtrl.PositionChanged += vlcCtrl_PositionChanged;
            vlcCtrl.EncounteredError += vlcCtrl_EncounteredError;

            media = new PathMedia(strFile);
            vlcCtrl.Show();             
            vlcCtrl.Play(media);
           
            GUIPropertyManager.SetProperty("#TV.Record.percent3", 0.0f.ToString()); // set to 0, as this player doesn't support download progress reporting

            GUIWaitCursor.Init(); GUIWaitCursor.Show(); // init and show the wait cursor while buffering

            return true;
        }

        void vlcCtrl_EncounteredError(VlcControl sender, VlcEventArgs<EventArgs> e) {
            string error = e.Data.ToString();
            GUIWindowManager.SendThreadCallbackAndWait((p1, p2, o) => {
                Log.Warn("VLCPlayer Error: '{0}'", error);
                if (!bufferingDone && Initializing) GUIWaitCursor.Hide(); // hide the wait cursor if still showing
                MediaPortal.Dialogs.GUIDialogOK dlg_error = (MediaPortal.Dialogs.GUIDialogOK)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_OK);
                if (dlg_error != null) {
                    dlg_error.Reset();
                    dlg_error.SetHeading(Settings.PLUGIN_NAME);                   
                    dlg_error.SetLine(1, error);
                    dlg_error.DoModal(GUIWindowManager.ActiveWindow);
                }
                PlaybackEnded();
                return 0;
            }, 0, 0, null);
        }

        void vlcCtrl_PositionChanged(VlcControl sender, VlcEventArgs<float> e) {
            playPosition = e.Data;
        }

        public override void Process() {
            if (media == null) return;

            if (media.State == States.Ended || media.State == States.Stopped)
                playState = PlayState.Ended;
            if (Initializing && media.State == States.Playing) playState = PlayState.Playing;

            if (!bufferingDone && !Initializing) {
                GUIWaitCursor.Hide(); // hide the wait cursor
                bufferingDone = true;

                if (Ended) {
                    PlaybackEnded();
                } else {
                    GUIWindowManager.ActivateWindow((int)GUIWindow.Window.WINDOW_FULLSCREEN_VIDEO);
                    GUIMessage msgPb = new GUIMessage(GUIMessage.MessageType.GUI_MSG_PLAYBACK_STARTED, 0, 0, 0, 0, 0, null);
                    msgPb.Label = CurrentFile;
                    GUIWindowManager.SendThreadMessage(msgPb);
                    SetVideoWindow();                   
                }
            }         
        }

        
        public override void SetVideoWindow() {
            vlcCtrl.Location = new Point(FullScreen ? 0 : GUIGraphicsContext.VideoWindow.X, FullScreen ? 0 : GUIGraphicsContext.VideoWindow.Y);
            vlcCtrl.ClientSize = new Size(FullScreen ? GUIGraphicsContext.Width : GUIGraphicsContext.VideoWindow.Width, FullScreen ? GUIGraphicsContext.Height : GUIGraphicsContext.VideoWindow.Height);
            _videoRectangle = new Rectangle(vlcCtrl.Location.X, vlcCtrl.Location.Y, vlcCtrl.ClientSize.Width, vlcCtrl.ClientSize.Height);
            _sourceRectangle = _videoRectangle;
        }

        private void PlaybackEnded() {
            Log.Info("VLCPlayer:ended {0}", url);
            playState = PlayState.Ended;
            if (vlcCtrl != null) {
                vlcCtrl.Location = new Point(0, 0);
                vlcCtrl.ClientSize = new Size(0, 0);
                vlcCtrl.Visible = false;
            }
        }

        public override void Pause() {
            if (media != null && playState == PlayState.Playing || playState == PlayState.Paused) {
                vlcCtrl.Pause();
                if (media.State == States.Paused)
                    playState = PlayState.Paused;
                else
                    playState = PlayState.Playing;
            }
        }

        public override void Stop() {
            if (vlcCtrl != null) {
                if (media.State != States.Stopped && media.State != States.Ended)
                    vlcCtrl.Stop();
                PlaybackEnded();
            }
        }

        public override double Duration {
            get { return media != null ? media.Duration.TotalSeconds : 0.0; }
        }

        public override double CurrentPosition {
            get { return playPosition * Duration; }
        }

        public override bool Initializing {
            get { return playState == PlayState.Init; }
        }

        public override bool Paused {
            get { return playState == PlayState.Paused; }
        }

        public override bool Playing {
            get { return !Ended; }
        }

        public override bool Stopped {
            get { return Initializing || Ended; }
        }

        public override bool Ended {
            get { return playState == PlayState.Ended; }
        }

        public override int Speed {
            get { return (Convert.ToInt32(vlcCtrl.Rate)); }
            set { vlcCtrl.Rate = value; }
        }

        public override void SeekRelative(double dTime) {
            double dCurTime = CurrentPosition;
            dTime = dCurTime + dTime;
            if (dTime < 0.0d) dTime = 0.0d;
            if (dTime < Duration) {
                SeekAbsolute(dTime);
            }
        }

        public override void SeekAbsolute(double dTime) {
            if (dTime < 0.0d) dTime = 0.0d;
            if (dTime < Duration) {
                if (vlcCtrl == null) return;
                try {
                    vlcCtrl.Time = TimeSpan.FromSeconds(dTime);
                } catch (Exception ex) { Log.Error(ex); }
            }
        }

        public override void SeekRelativePercentage(int iPercentage) {
            double dCurrentPos = CurrentPosition;
            double dCurPercent = (dCurrentPos / Duration) * 100.0d;
            double dOnePercent = Duration / 100.0d;
            dCurPercent = dCurPercent + (double)iPercentage;
            dCurPercent *= dOnePercent;
            if (dCurPercent < 0.0d) dCurPercent = 0.0d;
            if (dCurPercent < Duration) {
                SeekAbsolute(dCurPercent);
            }
        }

        public override void SeekAsolutePercentage(int iPercentage) {
            if (iPercentage < 0) iPercentage = 0;
            if (iPercentage >= 100) iPercentage = 100;
            double dPercent = Duration / 100.0f;
            dPercent *= (double)iPercentage;
            SeekAbsolute(dPercent);
        }

        public override string CurrentFile {
            get { return url; }
        }

        public override bool HasVideo {
            get { return true; }
        }

        public override bool HasViz {
            get { return true; }
        }

        public override bool IsCDA {
            get { return false; }
        }

        public override void Dispose() {
            if (!bufferingDone) GUIWaitCursor.Hide(); // hide the wait cursor            
            if (media != null) media.Dispose();
            if (vlcCtrl != null) vlcCtrl.Dispose();
        }

        #region OVSPLayer Member

        public bool GoFullscreen { get; set; }
        public string SubtitleFile { get; set; }

        #endregion


        static string vlcPath = null;
        public static bool IsInstalled {
            get {
                if (vlcPath == null) {
                    vlcPath = string.Empty;
                    Microsoft.Win32.RegistryKey regkeyVlcInstallPathKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\VideoLAN\VLC");
                    if (regkeyVlcInstallPathKey != null) {
                        string sVlcPath = (string)regkeyVlcInstallPathKey.GetValue("InstallDir", "");
                        if (Directory.Exists(sVlcPath)) vlcPath = sVlcPath;
                    }
                }
                return !string.IsNullOrEmpty(vlcPath);
            }
        }

        #region IPlayerFactory Members

        public IPlayer Create(string fileName, g_Player.MediaType type) {
            return this as IPlayer;
        }

        public IPlayer Create(string fileName) {
            return Create(fileName, g_Player.MediaType.Video);
        }

        #endregion
    }
}