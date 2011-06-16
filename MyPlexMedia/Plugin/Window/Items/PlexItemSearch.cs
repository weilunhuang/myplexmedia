using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyPlexMedia.Plugin.Config;

namespace MyPlexMedia.Plugin.Window.Items {
    class PlexItemSearch : PlexItemBase {

        private string Prompt { get; set; }
        
        public PlexItemSearch(IMenuItem parentItem, string title, Uri path, string userPrompt)
            : base(parentItem, title, path) {
            IsFolder = true;
            Prompt = userPrompt;
            IconImage = IconImageBig = ThumbnailImage = Settings.PLEX_ICON_DEFAULT_SEARCH;     
        }        

        public override void OnClicked(object sender, EventArgs e) {
            Uri originalUri = UriPath;
            string queryString = String.Format("&query={0}", Dialogs.GetKeyBoardInput("", Prompt));            
            UriPath = new Uri(originalUri.AbsoluteUri + queryString);
            base.OnClicked(sender, e);
            UriPath = originalUri;
        }
    }
}
