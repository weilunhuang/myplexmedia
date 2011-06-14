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
    class PlexItemDirectory : PlexItem{

        MediaContainerDirectory Directory { get; set; }

        public PlexItemDirectory(IMenuItem parentItem, string title, Uri path, MediaContainerDirectory directory) : base(parentItem, title, path) {
            Directory = directory;
            IsFolder = true;
            base.SetIcons(MediaRetrieval.GetArtWork(Directory.thumb ?? Directory.art ?? Settings.PLEX_ICON_DEFAULT));
            BackgroundImage = MediaRetrieval.GetArtWork(Directory.art ?? Directory.thumb);
        }
       
            
    }
}
