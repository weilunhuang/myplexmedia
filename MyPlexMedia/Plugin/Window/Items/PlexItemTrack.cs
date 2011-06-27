using System;
using MediaPortal.Music.Database;
using MediaPortal.Playlists;
using PlexMediaCenter.Plex.Data.Types;
using MediaPortal.Player;
using PlexMediaCenter.Util;
using PlexMediaCenter.Plex;
using MyPlexMedia.Plugin.Window.Playback;


namespace MyPlexMedia.Plugin.Window.Items {
    public class PlexItemTrack : PlexItemBase {

        public MediaContainerTrack Track { get; set; }
        public Uri PlaybackAuthUrl { get; set; }

        public PlexItemTrack(IMenuItem parentItem, string title, Uri path, MediaContainerTrack track)
            : base(parentItem, title, path) {
            Track = track;
            PlaybackAuthUrl = Transcoding.GetAuthPlaybackUrl(Track);
            IconImage = ThumbnailImage = (parentItem as PlexItemBase).IconImage;
            IconImageBig = (parentItem as PlexItemBase).IconImageBig;
        }

        public override void OnClicked(object sender, EventArgs e) {
            //MyPlexMediaPlayer myPlayer = new MyPlexMediaPlayer();           
            //myPlayer.PlayPlexItem(this);
            //g_Player.PlayAudioStream(PlaybackAuthUrl.AbsoluteUri);
            PlexPlayList.PlayItem(this);
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
