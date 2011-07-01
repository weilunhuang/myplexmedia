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
using PlexMediaCenter.Plex.Connection;

namespace MyPlexMedia.Plugin.Window.Items {
    internal class PlexItemServer : PlexItemBase {
        public PlexItemServer(IMenuItem parentItem, PlexServer plexServer)
            : base(parentItem, plexServer.HostAdress, plexServer.UriPlexSections) {
            IsFolder = true;

            PlexServer = plexServer;
            SetIcon(PlexServer.IsOnline ? Settings.PLEX_ICON_DEFAULT_ONLINE : Settings.PLEX_ICON_DEFAULT_OFFLINE);
            Label2 = (String.IsNullOrEmpty(plexServer.FriendlyName) ? plexServer.HostName : plexServer.FriendlyName);
        }

        private PlexServer PlexServer { get; set; }

        public override void OnClicked(object sender, EventArgs e) {
            Navigation.ShowRootMenu(PlexServer);
        }
    }
}