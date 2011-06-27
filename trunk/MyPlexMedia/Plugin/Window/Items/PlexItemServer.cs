﻿using System;
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
using MediaPortal.Util;
using PlexMediaCenter.Plex.Connection;

namespace MyPlexMedia.Plugin.Window.Items {
    class PlexItemServer : PlexItemBase {

        private PlexServer PlexServer { get; set; }

        public PlexItemServer(IMenuItem parentItem, PlexServer plexServer)
            : base(parentItem, plexServer.FriendlyName ?? plexServer.HostName, plexServer.UriPlexSections) {            
            IsFolder = true;

            PlexServer = plexServer;
            OnRetrieveArt += new RetrieveCoverArtHandler(PlexItemServer_OnRetrieveArt);
            string onOffImage = PlexServer.IsOnline ? Settings.PLEX_ICON_DEFAULT_ONLINE : Settings.PLEX_ICON_DEFAULT_OFFLINE;
            SetIcon(onOffImage);
            SetImage(onOffImage);
            Label2 = "@ " + PlexServer.HostAdress;        
        }

        void PlexItemServer_OnRetrieveArt(MediaPortal.GUI.Library.GUIListItem item) {
            string onOffImage = PlexServer.IsOnline ? Settings.PLEX_ICON_DEFAULT_ONLINE : Settings.PLEX_ICON_DEFAULT_OFFLINE;
            SetIcon(onOffImage);
            SetImage(onOffImage);
        }        
    }
}
