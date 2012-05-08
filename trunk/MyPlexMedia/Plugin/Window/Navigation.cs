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
using System.Linq;
using MyPlexMedia.Plugin.Config;
using MyPlexMedia.Plugin.Window.Dialogs;
using MyPlexMedia.Plugin.Window.Items;
using MyPlexMedia.Plugin.Window.Playback;
using PlexMediaCenter.Plex;
using PlexMediaCenter.Plex.Connection;
using PlexMediaCenter.Plex.Data.Types;

namespace MyPlexMedia.Plugin.Window {
    public static class Navigation {
        #region Delegates

        public delegate void OnErrorOccuredEventHandler(PlexException e);

        public delegate void OnMenuItemsFetchCompletedEventHandler(
            List<IMenuItem> fetchedMenuItems, int selectedFacadeIndex, Settings.PlexSectionLayout preferredLayout);

        public delegate void OnMenuItemsFetchStartedEventHandler(IMenuItem itemToFetch);

        #endregion

        static Navigation() {
            PlexInterface.ServerManager.OnPlexServersChanged += ServerManager_OnPlexServersChanged;
            PlexInterface.OnResponseReceived += PlexInterface_OnResponseReceived;
            CommonDialogs.OnProgressCancelled += CommonDialogs_OnProgressCancelled;
            History = new List<string> { Settings.PLUGIN_NAME };
            RootItem = new PlexItemBase(null, "Root Item", null);
            ServerItem = new MenuItem(RootItem, "Plex Servers");
            RootMenu = new List<IMenuItem> { ServerItem };
            RootItem.SetChildItems(RootMenu);
        }

        public static bool IsFetching { get; set; }

        public static PlexItemBase RootItem { get; set; }
        private static List<IMenuItem> RootMenu { get; set; }
        private static MenuItem ServerItem { get; set; }
        private static List<IMenuItem> ServerMenu { get; set; }
        public static IMenuItem CurrentItem { get; set; }
        public static List<string> History { get; set; }
        public static event OnErrorOccuredEventHandler OnErrorOccured;
        public static event OnMenuItemsFetchStartedEventHandler OnMenuItemsFetchStarted;
        public static event OnMenuItemsFetchCompletedEventHandler OnMenuItemsFetchCompleted;

        private static void CommonDialogs_OnProgressCancelled() {
            PlexInterface.RequestPlexItemsCancel();
        }

        public static void ShowRootMenu(PlexServer selectedServer) {
            MediaContainer plexSections = PlexInterface.TryGetPlexSections(selectedServer);
            if (plexSections == null) {
                return;
            }
            RootItem.UriPath = plexSections.UriSource;
            RootMenu = GetCreateSubMenuItems(RootItem, PlexInterface.RequestPlexItems(RootItem.UriPath));
            RootMenu.Add(new PlexItemSearch(RootItem, "Search...",
                                            new Uri(selectedServer.UriPlexBase, "search?type=0"),
                                            "Search Plex Server"));
            RootMenu.Add(ServerItem);
            RootItem.SetChildItems(RootMenu);
            ShowCurrentMenu(RootItem, 0);
        }

        internal static void CreateStartupMenu(PlexServer lastSelectedOrDefaultServer) {
            RefreshServerMenu();
            if (lastSelectedOrDefaultServer != null) {
                try {
                    ShowRootMenu(lastSelectedOrDefaultServer);
                    return;
                } catch (Exception e) {
                    OnErrorOccured(new PlexException(typeof(Navigation), "Creating startmenu failed!", e));
                }
            }
            ShowCurrentMenu(ServerItem, 0);
        }

        internal static void ShowCurrentContextMenu() {
            ContextMenu.ShowContextMenu(CurrentItem.Name, CurrentItem.ViewItems);
        }

        internal static void FetchPreviousMenu(IMenuItem currentItem, int storeLastSelectedFacadeIndex) {
            if (currentItem == null || currentItem.Parent == null) return;
            History.RemoveAt(History.Count - 1);
            currentItem.LastSelectedChildIndex = storeLastSelectedFacadeIndex;
            ShowCurrentMenu(currentItem.Parent, currentItem.Parent.LastSelectedChildIndex);
        }

        internal static void ShowCurrentMenu(IMenuItem parentItem, int selectFacadeIndex) {
            if (parentItem.ChildItems != null && parentItem.ChildItems.Count > 0) {
                CurrentItem = parentItem;
                PlexAudioPlayer.CreateMusicPlayList(
                    parentItem.ChildItems.Where(item => item is PlexItemTrack).ToList().ConvertAll(
                        item => item as PlexItemTrack), parentItem.Name);
                OnMenuItemsFetchCompleted(parentItem.ChildItems, selectFacadeIndex, parentItem.PreferredLayout);
            } else {
                return;
            }
        }

        internal static List<IMenuItem> GetCreateSubMenuItems(PlexItemBase parentItem,
                                                              MediaContainer plexResponseConatiner) {
            List<IMenuItem> tmpList = new List<IMenuItem>();
            try {
                if (plexResponseConatiner == null) {
                    throw new ArgumentNullException("plexResponseContainer");
                }
                //set item meta data
                parentItem.SetMetaData(plexResponseConatiner);
                parentItem.PreferredLayout = Settings.GetPreferredLayout(plexResponseConatiner.viewGroup);

                //We have a list of view items...
                tmpList.AddRange(plexResponseConatiner.Directory.Where(
                    dir => String.IsNullOrEmpty(dir.prompt)).Select<MediaContainerDirectory, IMenuItem>(
                        dir => new PlexItemDirectory(parentItem, dir.title, new Uri(parentItem.UriPath, dir.key), dir))
                    );
                //And a list of search items
                tmpList.AddRange(plexResponseConatiner.Directory.Where(
                    dir => !String.IsNullOrEmpty(dir.prompt)).Select<MediaContainerDirectory, IMenuItem>(
                        dir =>
                        new PlexItemSearch(parentItem, dir.prompt, new Uri(parentItem.UriPath, dir.key), dir.prompt))
                    );
                if (!string.IsNullOrEmpty(plexResponseConatiner.viewGroup) &&
                    plexResponseConatiner.viewGroup.Equals("secondary")) {
                    parentItem.ViewItems = tmpList;
                }
                plexResponseConatiner.Video.ForEach(vid => vid.parentIndex = plexResponseConatiner.parentIndex);
                tmpList.AddRange(
                    plexResponseConatiner.Video.ConvertAll<IMenuItem>(
                        vid => new PlexItemVideo(parentItem, vid.title, new Uri(parentItem.UriPath, vid.key), vid)));
                tmpList.AddRange(
                    plexResponseConatiner.Track.ConvertAll<IMenuItem>(
                        track =>
                        new PlexItemTrack(parentItem, track.title, new Uri(parentItem.UriPath, track.key), plexResponseConatiner.title1, plexResponseConatiner.title2, track)));
            } catch (Exception e) {
                OnErrorOccured(new PlexException(typeof(Navigation), "Creating submenu failed!", e));
            }
            return tmpList;
        }

        private static void PlexInterface_OnResponseReceived(object userToken, MediaContainer response) {
            if (userToken is PlexItemSearch && response.Directory.Count < 1 && response.Video.Count < 1 &&
                response.Track.Count < 1) {
                CommonDialogs.ShowNotifyDialog(10, "Plex Search", "Nothing found...", Settings.PLEX_ICON_DEFAULT_SEARCH, CommonDialogs.PLUGIN_NOTIFY_WINDOWS.WINDOW_DIALOG_OK);
            }
            if (userToken is PlexItemBase) {
                PlexItemBase item = userToken as PlexItemBase;
                List<IMenuItem> tmpChilds = GetCreateSubMenuItems(item, response);
                if (tmpChilds.Count > 0) {
                    item.SetChildItems(tmpChilds);
                } else {
                    CommonDialogs.ShowNotifyDialog(10, "Plex Request", "Nothing found...", Settings.PLEX_ICON_DEFAULT_SEARCH, CommonDialogs.PLUGIN_NOTIFY_WINDOWS.WINDOW_DIALOG_OK);
                    return;
                }
                History.Add(item.Name);
                ShowCurrentMenu(item, item.LastSelectedChildIndex);
            } else {
                OnErrorOccured(new PlexException(typeof(Navigation), "Unexpected item type in received response!",
                                                 new InvalidCastException()));
            }
        }

        private static void ServerManager_OnPlexServersChanged(List<PlexServer> updatedServerList) {
            ServerMenu = updatedServerList.ConvertAll<IMenuItem>(svr => new PlexItemServer(ServerItem, svr));
            ServerMenu.Add(new ActionItem(null, "Refresh Bonjouor...", Settings.PLEX_ICON_DEFAULT_BONJOUR,
                                          RefreshServerMenu));
            ServerMenu.Add(new ActionItem(null, "Add Plex Server...", Settings.PLEX_ICON_DEFAULT_ONLINE,
                                          AddNewPlexServer));
            ServerItem.SetChildItems(ServerMenu);
            if (CurrentItem == ServerItem) {
                ShowCurrentMenu(ServerItem, 0);
            }
        }

        internal static void RefreshServerMenu() {
            PlexInterface.ServerManager.RefreshBonjourServers();
        }

        internal static void AddNewPlexServer() {
            try {
                ManualConnectionInfo newServer = new ManualConnectionInfo(
                    CommonDialogs.GetKeyBoardInput("Friendly name", "HostName"),
                    CommonDialogs.GetKeyBoardInput("Host Adress", "HostAdress"),
                    int.Parse(CommonDialogs.GetKeyBoardInput("Plex Port", "PlexPort")),
                    CommonDialogs.GetKeyBoardInput("Login", "UserName"),
                    CommonDialogs.GetKeyBoardInput("Password", "UserPass"));

                if (!PlexInterface.ServerManager.TryAddManualServerConnection(newServer)) {
                    if (CommonDialogs.ShowCustomYesNo("PlexServer not found!",
                                                      "The new PlexServer appears to be offline \nor was misconfigured...",
                                                      "Try Again!", "Cancel", false)) {
                        AddNewPlexServer();
                    }
                }
            } catch {
                ShowCurrentMenu(RootItem, 0);
            }
        }

        internal static void RequestChildItems(Uri uriPath, IMenuItem destObject) {
            OnMenuItemsFetchStarted(destObject);
            PlexInterface.RequestPlexItemsAsync(uriPath, destObject);
        }
    }
}