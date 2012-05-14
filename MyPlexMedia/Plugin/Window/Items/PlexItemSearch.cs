#region #region Copyright (C) 2005-2011 Team MediaPortal

// 
// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.
// 

#endregion

using System;
using MyPlexMedia.Plugin.Config;
using MyPlexMedia.Plugin.Window.Dialogs;

namespace MyPlexMedia.Plugin.Window.Items {
    public class PlexItemSearch : PlexItemBase {
        public PlexItemSearch(IMenuItem parentItem, string title, Uri path, string userPrompt)
            : base(parentItem, title, path) {
            Prompt = userPrompt;
            SetIcon(Settings.PLEX_ICON_DEFAULT_SEARCH);
            RetrieveArt = false;
        }

        private string Prompt { get; set; }

        public override void OnClicked(object sender, EventArgs e) {
            ChildItems = null;
            Uri originalUri = UriPath;
            string queryString = String.Format("&query={0}", CommonDialogs.GetKeyBoardInput("", Prompt));
            UriPath = new Uri(originalUri.AbsoluteUri + queryString);
            base.OnClicked(sender, e);
            UriPath = originalUri;
        }
    }
}