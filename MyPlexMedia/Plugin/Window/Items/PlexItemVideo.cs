using System;
using MediaPortal.GUI.Library;
using MediaPortal.GUI.Video;
using MediaPortal.Video.Database;
using PlexMediaCenter.Plex;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Util;
using MediaPortal.Player;
using MyPlexMedia.Plugin.Window.Dialogs;

namespace MyPlexMedia.Plugin.Window.Items {
    public class PlexItemVideo : PlexItemBase {

        public MediaContainerVideo Video { get; set; }

        public PlexItemVideo(IMenuItem parentItem, string title, Uri path, MediaContainerVideo video)
            : base(parentItem, title, path) {
            Video = video;

            PlexInterface.ArtworkRetriever.QueueArtwork(SetIcon, PlexInterface.PlexServerCurrent, Video.thumb);
            PlexInterface.ArtworkRetriever.QueueArtwork(SetImage, PlexInterface.PlexServerCurrent, Video.art);

            int duration;
            if (int.TryParse(Video.duration, out duration)) {
                base.Duration = duration;
            }
            if (!string.IsNullOrEmpty(Video.rating)) {
                try {
                    Rating = float.Parse(Video.rating);
                } catch { }
            }
            int year;
            if (int.TryParse(Video.year, out year)) {
                base.Year = year;
            }
            FileInfo = new MediaPortal.Util.FileInformation();
            if (!String.IsNullOrEmpty(Video.originallyAvailableAt)) {
                FileInfo.CreationTime = DateTime.Parse(Video.originallyAvailableAt);
                Label2 = FileInfo.CreationTime.ToShortDateString();
            }
        }

        public override void OnClicked(object sender, EventArgs e) {
            
            string playBackUrl = Transcoding.GetFlvStreamUrl(PlexInterface.PlexServerCurrent, Video.Media[0].Part[0].key).AbsoluteUri;
            BaseStreamBufferPlayer bsbp = new BaseStreamBufferPlayer();
            StreamBufferPlayer9 sbp9 = new StreamBufferPlayer9();
            RTSPPlayer rtsp = new RTSPPlayer();
            
            if (bsbp.PlayStream(playBackUrl, Video.title)) {

            } else if (sbp9.PlayStream(playBackUrl, Video.title)) {

            } else if (rtsp.PlayStream(playBackUrl, Video.title)) {

            } else {
                g_Player.Init();
                g_Player.SetVideoWindow();

                Transcoding.OnPlayBufferedMedia += new Transcoding.OnPlayBufferedMediaEventHandler(Transcoding_OnPlayBufferedMedia);
                Transcoding.OnBufferingProgress += new Transcoding.OnBufferingProgressEventHandler(Transcoding_OnBufferingProgress);
                g_Player.PlayBackEnded += new g_Player.EndedHandler(g_Player_PlayBackEnded);
                CommonDialogs.ShowWaitCursor();
                GUIWindowManager.Process();
                //Transcoding.PlayBackMedia(Video);
                Transcoding.Buf
            }
        }

        void g_Player_PlayBackEnded(g_Player.MediaType type, string filename) {
            if (Transcoding.IsBuffering) {
                g_Player.Pause();
            }
        }

        void Transcoding_OnBufferingProgress(int currentProgress) {
            CommonDialogs.ShowProgressDialog("Buffering...", Video.title, currentProgress);
            if (g_Player.Playing && g_Player.Paused) {
                g_Player.Pause();
            }
        }

        void Transcoding_OnPlayBufferedMedia(string localBufferPath) {
            CommonDialogs.HideProgressDialog();
            g_Player.Init();
            g_Player.SetVideoWindow();
            g_Player.PlayVideoStream(localBufferPath, Video.title);
        }


        public override void OnSelected() {

        }

        public override void OnInfo() {
            IMDBMovie movieDetails = new IMDBMovie();
            movieDetails.Plot = Video.summary;
            movieDetails.ThumbURL = IconImage;
            movieDetails.PlotOutline = Video.tagline;
            movieDetails.Title = Video.title;
            movieDetails.RunTime = Duration;
            movieDetails.Rating = Rating;
            movieDetails.Year = Year;
            movieDetails.MPARating = Video.contentRating;

            GUIVideoInfo videoInfo = (GUIVideoInfo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_VIDEO_INFO);
            videoInfo.Movie = movieDetails;
            GUIWindowManager.ActivateWindow((int)GUIWindow.Window.WINDOW_VIDEO_INFO);
        }
    }
}
