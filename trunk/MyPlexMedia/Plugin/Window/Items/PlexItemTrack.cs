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
using MediaPortal.Music.Database;


namespace MyPlexMedia.Plugin.Window.Items {
    class PlexItemTrack : PlexItemBase {

        public MediaContainerTrack Track { get; set; }
        public PlayListItem PlayListItem { get; set; }

        public PlexItemTrack(IMenuItem parentItem, string title, Uri path, MediaContainerTrack track)
            : base(parentItem, title, path) {
            Track = track;
            IconImage = ThumbnailImage = (parentItem as PlexItemBase).IconImage;
            IconImageBig = (parentItem as PlexItemBase).IconImageBig;
        }

        public override void OnClicked(object sender, EventArgs e) {
            //g_Player.PlayAudioStream(PlexInterface.GetPlayBackProxyUrl(PlexInterface.PlexServerCurrent.UriPlexBase + Track.Media[0].Part[0].key));
        }

        public override void OnSelected() {

        }

        public override void OnInfo() {
            Song track = new Song();
            track.Album = Parent.Name;
        }

    }
}
