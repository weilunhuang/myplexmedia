using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using PlexMediaCenter.Plex.Data;
using PlexMediaCenter.Util;
using MyPlexMedia.Plugin.Config;
using PlexMediaCenter.Plex;
using PlexMediaCenter.Plex.Data.Types;

namespace MyPlexMedia.Plugin.Window.Items {
    class PlexItemDirectory : PlexItemBase{        

        MediaContainerDirectory Directory { get; set; }

        public PlexItemDirectory(IMenuItem parentItem, string title, Uri path, MediaContainerDirectory directory) : base(parentItem, title, path) {
            Directory = directory;
            IsFolder = true;
            
            int duration;
            if(int.TryParse(Directory.duration, out duration)){
                base.Duration = duration;
            }
            if (!String.IsNullOrEmpty(Directory.viewedLeafCount) && !String.IsNullOrEmpty(Directory.leafCount)) {
                Label2 += String.Format(" [{0}/{1}]", Directory.viewedLeafCount, Directory.leafCount);
            }
            Label3 = Directory.summary;
            FileInfo = new MediaPortal.Util.FileInformation();
            if (!String.IsNullOrEmpty(Directory.originallyAvailableAt)) {
                FileInfo.CreationTime = DateTime.Parse(Directory.originallyAvailableAt);
            }
        }

        protected override void OnRetrieveArtwork(MediaPortal.GUI.Library.GUIListItem item) {
            IconImage = PlexInterface.ArtworkRetriever.GetArtwork(Directory.thumb);
            IconImageBig = PlexInterface.ArtworkRetriever.GetArtwork(Directory.art);
            ThumbnailImage = PlexInterface.ArtworkRetriever.GetArtwork(Directory.thumb);
            BackgroundImage = !String.IsNullOrEmpty(Directory.art) ? PlexInterface.ArtworkRetriever.GetArtwork(Directory.art) : string.Empty;            
        }
    }
}
