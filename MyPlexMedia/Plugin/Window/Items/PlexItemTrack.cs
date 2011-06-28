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
using MediaPortal.Music.Database;
using MyPlexMedia.Plugin.Window.Playback;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Util;

namespace MyPlexMedia.Plugin.Window.Items {
    public class PlexItemTrack : PlexItemBase {
        public PlexItemTrack(IMenuItem parentItem, string title, Uri path, MediaContainerTrack track)
            : base(parentItem, title, path) {
            Track = track;
            PlaybackAuthUrl = Transcoding.GetTrackPlaybackUrl(UriPath, Track);
            IconImage = ThumbnailImage = ((PlexItemBase) parentItem).IconImage;
            if (parentItem != null) IconImageBig = (parentItem as PlexItemBase).IconImageBig;
        }

        public MediaContainerTrack Track { get; set; }
        public Uri PlaybackAuthUrl { get; set; }

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
            Song track = new Song {Album = Parent.Name};
        }
    }
}