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
            IconImage = MediaRetrieval.GetArtWork(Directory.thumb);
            IconImageBig = MediaRetrieval.GetArtWork(Directory.art);
            ThumbnailImage = MediaRetrieval.GetArtWork(Directory.thumb);
            BackgroundImage = !String.IsNullOrEmpty(Directory.art) ? MediaRetrieval.GetArtWork(Directory.art) : string.Empty;
            
            int duration;
            if(int.TryParse(Directory.duration, out duration)){
                base.Duration = duration;
            }
            Label2 += String.Format(" [{0}/{1}]", Directory.viewedLeafCount, Directory.leafCount);            
        }               
            
    }
}
