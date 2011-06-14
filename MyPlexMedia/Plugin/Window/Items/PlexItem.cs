using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using PlexMediaCenter.Util;
using PlexMediaCenter.Plex;


namespace MyPlexMedia.Plugin.Window.Items {
    class PlexItem : MenuItem {        

        public PlexItem(IMenuItem parentItem, string title, Uri path) : base(parentItem, title) {
            if (path != null) {
                UriPath = path.AbsoluteUri.Contains("?") ? path : new Uri((path.AbsoluteUri).EndsWith("/") ? path.AbsoluteUri : path.AbsoluteUri + "/");
            }
            //base.SetIcons(MediaRetrieval.GetArtWork(UriPath.AbsoluteUri));
        }
       

        public override void OnClicked(object sender, EventArgs e) {
            try {
                SetChildItems(Navigation.GetSubMenuItems(this, PlexInterface.RequestPlexItems(UriPath)));
                Navigation.ShowCurrentMenu(this);
            } catch {
              
            }            
        }
    }
}
