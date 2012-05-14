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
using MediaPortal.GUI.Music;
using MediaPortal.GUI.Library;

namespace MyPlexMedia.Plugin.Window.Items {
    public class PlexItemTrack : PlexItemBase {
        public PlexItemTrack(IMenuItem parentItem, string title, Uri path, string artist, string album, MediaContainerTrack track)
            : base(parentItem, title, path) {
            Track = track;
            PlaybackAuthUrl = Transcoding.GetTrackPlaybackUrl(UriPath, Track);
            if (parentItem != null) {
                IconImage = ((PlexItemBase)parentItem).IconImage;
                ThumbnailImage = ((PlexItemBase)parentItem).ThumbnailImage;
                IconImageBig = ((PlexItemBase)parentItem).IconImageBig;
                BackgroundImage = ((PlexItemBase)parentItem).BackgroundImage;
            }
            int duration = 0;
            int.TryParse(track.duration, out duration);
            Label2 = album;
            int index = 0;
            int.TryParse(track.index, out index);
            Song song = new Song() {
                Artist = artist,
                Album = album,
                Duration = duration,
                Track = index,
                WebImage = IconImage,
                Title = title,
                Codec = track.Media[0].audioCodec,
                FileType = track.Media[0].container,
                FileName = track.Media[0].Part[0].file
            };
            MusicTag = song;
        }

        public MediaContainerTrack Track { get; set; }
        public Uri PlaybackAuthUrl { get; set; }

        public override void OnClicked(object sender, EventArgs e) {
            //MyPlexMediaPlayer myPlayer = new MyPlexMediaPlayer();           
            //myPlayer.PlayPlexItem(this);
            //g_Player.PlayAudioStream(PlaybackAuthUrl.AbsoluteUri);
            PlexAudioPlayer.PlayItem(this);
            //g_Player.PlayAudioStream(PlexInterface.GetPlayBackProxyUrl(PlexInterface.PlexServerCurrent.UriPlexBase + Track.Media[0].Part[0].key));
        }
    }
}