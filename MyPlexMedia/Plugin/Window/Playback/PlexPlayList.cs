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

using System.Collections.Generic;
using System.Linq;
using MediaPortal.GUI.Library;
using MediaPortal.Playlists;
using MyPlexMedia.Plugin.Window.Items;
using Action = System.Action;

namespace MyPlexMedia.Plugin.Window.Playback {
    public static class PlexPlayList {
        private static readonly PlayListPlayer PlexPlayListPlayer;

        static PlexPlayList() {
            PlexPlayListPlayer = PlayListPlayer.SingletonPlayer;
            PlexPlayListPlayer.Init();
        }

        public static void CreateMusicPlayList(List<PlexItemTrack> currentTracks, string listTitle) {
            if (currentTracks == null || currentTracks.Count < 1) {
                return;
            }
            PlayList newPlayList = new PlayList { Name = listTitle };
            foreach (
                var newItem in
                    currentTracks.Select(
                        currentTrack =>
                        new PlayListItem(currentTrack.Track.title, currentTrack.PlaybackAuthUrl.AbsoluteUri,
                                         int.Parse(currentTrack.Track.duration)) { Type = PlayListItem.PlayListItemType.AudioStream })) {
                newPlayList.Add(newItem);
            }
            PlexPlayListPlayer.ReplacePlaylist(PlayListType.PLAYLIST_MUSIC, newPlayList);
            PlexPlayListPlayer.Reset();
        }

        public static void PlayItem(PlexItemTrack track) {
            PlayItem(track.PlaybackAuthUrl.AbsoluteUri);
        }

        private static void PlayItem(string fileNameUri) {
            if (GUIGraphicsContext.form.InvokeRequired) {
                GUIGraphicsContext.form.Invoke(new Action(() => PlayItem(fileNameUri)));
            }
            PlexPlayListPlayer.g_Player.PlayAudioStream(fileNameUri);
        }
    }
}