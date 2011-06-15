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
    class PlexItemVideo : PlexItem {

        public MediaContainerVideo Video { get; set; }

        public PlexItemVideo(IMenuItem parentItem, string title, Uri path, MediaContainerVideo video)
            : base(parentItem, title, path) {
            Video = video;            
            IconImage = MediaRetrieval.GetArtWork(Video.thumb);
            IconImageBig = MediaRetrieval.GetArtWork(Video.thumb);
            ThumbnailImage = MediaRetrieval.GetArtWork(Video.thumb);
            BackgroundImage = !String.IsNullOrEmpty(Video.art) ? MediaRetrieval.GetArtWork(Video.art) : string.Empty;
        }     

        public override void OnClicked(object sender, EventArgs e) {
        
           g_Player.PlayVideoStream(Transcoding.GetM3U8PlaylistUrl(PlexInterface.PlexServerCurrent, PlexInterface.GetAllVideoPartKeys(Video).First()).AbsoluteUri);
           MediaPortal.Player.g_Player.ShowFullScreenWindow();
        }

        public override void OnSelected() {
            
        }
    }
}
