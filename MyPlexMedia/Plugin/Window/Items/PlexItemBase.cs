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
using System.Collections.Generic;
using MediaPortal.GUI.Library;
using MediaPortal.Util;
using MyPlexMedia.Plugin.Config;
using PlexMediaCenter.Plex.Data.Types;

namespace MyPlexMedia.Plugin.Window.Items {
    public class PlexItemBase : MenuItem {
        #region Delegates

        public delegate void OnHasBackgroundEventHandler(string imagePath);

        #endregion

        public PlexItemBase(IMenuItem parentItem, string title, Uri path)
            : base(parentItem, title) {
            if (path != null) {
                UriPath = path.AbsoluteUri.Contains("?")
                              ? path
                              : new Uri((path.AbsoluteUri).EndsWith("/") ? path.AbsoluteUri : path.AbsoluteUri + "/");
            }
            ViewItems = new List<IMenuItem>();
        }

        protected virtual void SetIcon(string imagePath) {
            //if (GUITextureManager.Load(imagePath, 0, 0, 0, true) > 0) {
                IconImageBig = IconImage = ThumbnailImage = imagePath;
                //RetrieveArt = false;
            //}
        }

        protected virtual void SetBackground(string imagePath) {
            //if (GUITextureManager.Load(imagePath, 0, 0, 0, true) > 0) {
                BackgroundImage = imagePath;
                //RetrieveArt = false;
            //}
        }

        public virtual void SetMetaData(MediaContainer infoContainer) {
            int year;
            if (int.TryParse(infoContainer.parentYear, out year)) {
                Year = year;
            }
            AlbumInfoTag = infoContainer;
        }

        public override void OnClicked(object sender, EventArgs e) {
            if (ChildItems == null || ChildItems.Count < 1) {
                Navigation.RequestChildItems(UriPath, this);
            } else {
                Navigation.ShowCurrentMenu(this, LastSelectedChildIndex);
            }
        }
    }
}