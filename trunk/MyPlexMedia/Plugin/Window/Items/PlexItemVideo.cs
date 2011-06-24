using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Util;
using MyPlexMedia.Plugin.Config;
using PlexMediaCenter.Plex;
using MediaPortal.Player;
using MediaPortal.Playlists;
using MyPlexMedia.Plugin.Player;
using MediaPortal.GUI.Video;
using MediaPortal.GUI.Library;
using MediaPortal.Video.Database;

namespace MyPlexMedia.Plugin.Window.Items {
    class PlexItemVideo : PlexItemBase {

        public MediaContainerVideo Video { get; set; }

        public PlexItemVideo(IMenuItem parentItem, string title, Uri path, MediaContainerVideo video)
            : base(parentItem, title, path) {
            Video = video;           

            PlexInterface.ArtworkRetriever.QueueArtwork(SetIcon, UriPath, Video.thumb);
            PlexInterface.ArtworkRetriever.QueueArtwork(SetImage, UriPath, Video.art);

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
            
            MyPlexMediaPlayer myPlayer = new MyPlexMediaPlayer();
            myPlayer.FullScreen = true;
            myPlayer.GoFullscreen = true;            
            myPlayer.PlaySegmentedVideoStream(Transcoding.GetM3U8PlaylistUrl(PlexInterface.PlexServerCurrent, Video.Media[0].Part[0].key, 0, 1, false));            
            myPlayer.Process();

            //g_Player.PlayVideoStream(Transcoding.GetM3U8PlaylistUrl(PlexInterface.PlexServerCurrent, Video.Media[0].Part[0].key, 0, 1, false).AbsoluteUri);
            
           
            //g_Player.ShowFullScreenWindow();
            ////AXVLC.VLCPlugin2Class plugin = new AXVLC.VLCPlugin2Class();
            ////plugin.Visible = true;
            //plugin.addTarget(Transcoding.GetM3U8PlaylistUrl(PlexInterface.PlexServerCurrent, Video.Media[0].Part[0].key, 0, 1, true).AbsoluteUri, Type.Missing, AXVLC.VLCPlaylistMode.VLCPlayListReplace, 0);
            ////plugin.play();
            //string test = Transcoding.GetFlvStreamUrl(PlexInterface.PlexServerCurrent, Video.Media[0].Part[0].key).AbsoluteUri;
            //VideoPlayerVMR9 t = new VideoPlayerVMR9(g_Player.MediaType.Video);
            //t.FullScreen = true;
            //t.PlayStream(test, "Test");
            //g_Player.PlayVideoStream(test);
            //g_Player.ShowFullScreenWindow();
           // Transcoding.PlayBackMedia(Video);
        }        
       

        public override void OnSelected() {

        }

        public override void OnInfo() {
            IMDBMovie movieDetails = new IMDBMovie();
            movieDetails.Plot = Video.summary;
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
