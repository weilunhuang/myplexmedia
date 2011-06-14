using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Util;
using MyPlexMedia.Plugin.Config;

namespace MyPlexMedia.Plugin.Window.Items {
    class PlexItemVideo : PlexItem {

        public MediaContainerVideo Video { get; set; }

        public PlexItemVideo(IMenuItem parentItem, string title, Uri path, MediaContainerVideo video)
            : base(parentItem, title, path) {
            Video = video;
            base.SetIcons(MediaRetrieval.GetArtWork(Video.thumb ?? Video.art ?? Settings.PLEX_ICON_DEFAULT));
            BackgroundImage = MediaRetrieval.GetArtWork(Video.art ?? Video.thumb);
        }     

        public override void OnClicked(object sender, EventArgs e) {
            Transcoding.PlayBackMedia(this.Video);
        }

        public override void OnSelected() {
            
        }
    }
}
