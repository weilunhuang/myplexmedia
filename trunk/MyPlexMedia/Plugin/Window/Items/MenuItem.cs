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
using MyPlexMedia.Plugin.Config;

namespace MyPlexMedia.Plugin.Window.Items {
    public class MenuItem : GUIListItem, IMenuItem {
        #region Delegates

        public delegate void OnMenuItemSelectedEventHandler(IMenuItem selectedItem);

        #endregion

        public MenuItem(IMenuItem parent, string title) : base(title) {
            Parent = parent;
            OnItemSelected += MenuItem_OnItemSelected;
            IconImage = Settings.PLEX_ICON_DEFAULT;
            IconImageBig = Settings.PLEX_ICON_DEFAULT;
            ThumbnailImage = Settings.PLEX_ICON_DEFAULT;
        }

        #region IMenuItem Members

        public List<IMenuItem> ViewItems { get; set; }

        public string Name {
            get { return Label; }
        }

        public string BackgroundImage { get; set; }
        public IMenuItem Parent { get; set; }
        public Uri UriPath { get; set; }
        public int LastSelectedChildIndex { get; set; }

        public List<IMenuItem> ChildItems { get; set; }

        public virtual void OnClicked(object sender, EventArgs e) {
            if (ChildItems != null && ChildItems.Count > 0) {
                Navigation.ShowCurrentMenu(this, 0);
            }
        }

        public virtual void OnSelected() {
            OnMenuItemSelected(this);
        }

        public virtual void OnInfo() {
        }

        public GUIFacadeControl.Layout PreferredLayout { get; set; }

        #endregion

        public static event OnMenuItemSelectedEventHandler OnMenuItemSelected;

        private void MenuItem_OnItemSelected(GUIListItem item, GUIControl parent) {
            OnSelected();
        }

        public void SetChildItems(List<IMenuItem> childItems) {
            childItems.ForEach(ch => ch.Parent = this);
            ChildItems = childItems;
        }
    }
}