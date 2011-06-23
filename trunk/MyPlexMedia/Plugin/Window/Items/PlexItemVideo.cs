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

namespace MyPlexMedia.Plugin.Window.Items {
    class PlexItemVideo : PlexItemBase {

        public MediaContainerVideo Video { get; set; }

        public PlexItemVideo(IMenuItem parentItem, string title, Uri path, MediaContainerVideo video)
            : base(parentItem, title, path) {
            Video = video;

            PlexInterface.ArtworkRetriever.QueueArtwork(SetIcon, Video.thumb);
            PlexInterface.ArtworkRetriever.QueueArtwork(SetImage, Video.art);

            int duration;
            if (int.TryParse(Video.duration, out duration)) {
                base.Duration = duration;
            }
            if (!string.IsNullOrEmpty(Video.rating)) {
                try {
                    Rating = float.Parse(Video.rating);
                } catch { }
            }
            FileInfo = new MediaPortal.Util.FileInformation();
            if (!String.IsNullOrEmpty(Video.originallyAvailableAt)) {
                FileInfo.CreationTime = DateTime.Parse(Video.originallyAvailableAt);
                Label2 = FileInfo.CreationTime.ToShortDateString();
            }
        }     

        public override void OnClicked(object sender, EventArgs e) {
            Transcoding.OnPlayBufferedMedia += new Transcoding.OnPlayBufferedMediaEventHandler(Transcoding_OnPlayBufferedMedia);
            Transcoding.OnPlayHttpAdaptiveStream += new Transcoding.OnPlayHttpAdaptiveStreamEventHandler(Transcoding_OnPlayHttpAdaptiveStream);
            List<String> list =  Transcoding.GetM3U8PlaylistItems(PlexInterface.PlexServerCurrent, Video.Media[0].Part[0].key).ToList();

            MyPlexMediaPlayer myPlayer = new MyPlexMediaPlayer();
            myPlayer.Play(Transcoding.GetM3U8PlaylistUrl(PlexInterface.PlexServerCurrent, Video.Media[0].Part[0].key, 0, 1, false).AbsoluteUri);


            //AXVLC.VLCPlugin2Class plugin = new AXVLC.VLCPlugin2Class();
            //plugin.Visible = true;
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
        
        void Transcoding_OnPlayHttpAdaptiveStream(Uri m3u8Url) {
            throw new NotImplementedException();
        }

        void Transcoding_OnPlayBufferedMedia(string localBufferPath) {
            g_Player.PlayVideoStream(localBufferPath);
            g_Player.ShowFullScreenWindow();
        }

        public override void OnSelected() {

        }
    }
}
