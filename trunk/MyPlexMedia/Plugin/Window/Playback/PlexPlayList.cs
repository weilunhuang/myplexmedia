using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.Player;
using MediaPortal.Playlists;
using MyPlexMedia.Plugin.Window.Items;
using PlexMediaCenter.Util;
using MediaPortal.GUI.Library;
using System.Windows.Forms;
using MyPlexMedia.Plugin.Config;

namespace MyPlexMedia.Plugin.Window.Playback {
    public static class PlexPlayList {

        private static PlayListPlayer PlexPlayListPlayer;

       static PlexPlayList() {
            PlexPlayListPlayer = PlayListPlayer.SingletonPlayer;
            PlexPlayListPlayer.Init();
        }

        public static void CreatePlayList(List<PlexItemTrack> currentTracks, string listTitle) {                     
            PlayList newPlayList = new PlayList();
            newPlayList.Name = listTitle;
            foreach (PlexItemTrack currentTrack in currentTracks) {
                PlayListItem newItem = new PlayListItem(currentTrack.Track.title, currentTrack.PlaybackAuthUrl.AbsoluteUri, int.Parse(currentTrack.Track.duration));
                newItem.Type = PlayListItem.PlayListItemType.AudioStream;
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
                GUIGraphicsContext.form.Invoke(new System.Action(()=> PlayItem(fileNameUri)));
            }
            
            PlexPlayListPlayer.Play(fileNameUri);
            if (Settings.PLUGIN_WINDOW_ID == GUIWindowManager.ActiveWindow) {
                GUIWindowManager.ActivateWindow((int)GUIWindow.Window.WINDOW_MUSIC_PLAYLIST);
            }
        }
    }
}
