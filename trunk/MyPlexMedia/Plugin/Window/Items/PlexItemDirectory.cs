using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Util;

namespace MyPlexMedia.Plugin.Window.Items {
    class PlexItemDirectory : PlexItem{

        MediaContainerDirectory Directory { get; set; }

        public PlexItemDirectory(IMenuItem parentItem, string title, Uri path, MediaContainerDirectory directory) : base(parentItem, title, path) {
            Directory = directory;
            base.SetIcons(MediaRetrieval.GetArtWork(Directory.thumb));
            BackgroundImage = MediaRetrieval.GetArtWork(Directory.art ?? Directory.thumb);
        }
       
        public override void OnSelected() {           
        }

      
    }
}
