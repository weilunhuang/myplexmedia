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
            try {
                Rating = float.Parse(Video.rating);
            } catch { }
            FileInfo = new MediaPortal.Util.FileInformation();
            if (!String.IsNullOrEmpty(Video.originallyAvailableAt)) {
                FileInfo.CreationTime = DateTime.Parse(Video.originallyAvailableAt);
                Label2 = FileInfo.CreationTime.ToShortDateString();
            }
        }     

        public override void OnClicked(object sender, EventArgs e) {        
           g_Player.PlayVideoStream(Transcoding.GetM3U8PlaylistUrl(PlexInterface.PlexServerCurrent, PlexInterface.GetAllVideoPartKeys(Video).First()).AbsoluteUri);
           MediaPortal.Player.g_Player.ShowFullScreenWindow();
        }

        public override void OnSelected() {
            
        }
    }
}
