using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyPlexMedia.Plugin.Config;
using MyPlexMedia.Plugin.Window.Dialogs;

namespace MyPlexMedia.Plugin.Window.Items {
   public class PlexItemSearch : PlexItemBase {

        private string Prompt { get; set; }
        
        public PlexItemSearch(IMenuItem parentItem, string title, Uri path, string userPrompt)
            : base(parentItem, title, path) {
            IsFolder = true;
            Prompt = userPrompt;
            IconImage = Settings.PLEX_ICON_SEARCH;
            IconImageBig = Settings.PLEX_ICON_SEARCH;
            ThumbnailImage = Settings.PLEX_ICON_SEARCH;
        }        

        public override void OnClicked(object sender, EventArgs e) {
            Uri originalUri = UriPath;
            string queryString = String.Format("&query={0}", CommonDialogs.GetKeyBoardInput("", Prompt));            
            UriPath = new Uri(originalUri.AbsoluteUri + queryString);
            base.OnClicked(sender, e);
            UriPath = originalUri;
        }
    }
}
