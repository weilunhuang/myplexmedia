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

namespace MyPlexMedia.Plugin.Window.Items {
    class PlexItemVideo : PlexItemBase {

        public MediaContainerVideo Video { get; set; }

        public PlexItemVideo(IMenuItem parentItem, string title, Uri path, MediaContainerVideo video)
            : base(parentItem, title, path) {
            Video = video;
            IconImage = PlexInterface.ArtworkRetriever.GetArtwork(Video.thumb);
            IconImageBig = PlexInterface.ArtworkRetriever.GetArtwork(Video.thumb);
            ThumbnailImage = PlexInterface.ArtworkRetriever.GetArtwork(Video.thumb);
            BackgroundImage = !String.IsNullOrEmpty(Video.art) ? PlexInterface.ArtworkRetriever.GetArtwork(Video.art) : string.Empty;

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
           
            Transcoding.PlayBackMedia(Video);
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
