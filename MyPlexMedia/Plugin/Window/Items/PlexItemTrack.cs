using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Util;
using MyPlexMedia.Plugin.Config;
using MediaPortal.Playlists;
using PlexMediaCenter.Plex;
using MediaPortal.Player;

namespace MyPlexMedia.Plugin.Window.Items {
    class PlexItemTrack : PlexItemBase {

        public MediaContainerTrack Track { get; set; }
        public PlayListItem PlayListItem { get; set; }

        public PlexItemTrack(IMenuItem parentItem, string title, Uri path, MediaContainerTrack track)
            : base(parentItem, title, path) {
            Track = track;
            IconImage = IconImageBig = ThumbnailImage = Settings.PLEX_ICON_DEFAULT;

        }

        public override void OnClicked(object sender, EventArgs e) {                 
            g_Player.PlayAudioStream(PlexInterface.GetPlayBackProxyUrl(PlexInterface.PlexServerCurrent.UriPlexBase + Track.Media[0].Part[0].key));
        }

        public override void OnSelected() {

        }
    }
}
