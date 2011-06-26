using System;
using System.Collections.Generic;
using MyPlexMedia.Plugin.Window.Items;
using PlexMediaCenter.Plex;
using PlexMediaCenter.Plex.Connection;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Util;
using System.Linq;
using MyPlexMedia.Plugin.Config;
using MediaPortal.GUI.Library;
using System.Net;
using MyPlexMedia.Plugin.Window.Dialogs;
using System.IO;
using System.Collections;

namespace MyPlexMedia.Plugin.Window {
    public static class Navigation {
        public static bool IsFetching { get; set; }

        public static event OnErrorOccuredEventHandler OnErrorOccured;
        public delegate void OnErrorOccuredEventHandler(PlexException e);

        public static event OnMenuItemsFetchStartedEventHandler OnMenuItemsFetchStarted;
        public delegate void OnMenuItemsFetchStartedEventHandler(IMenuItem itemToFetch);

        public static event OnMenuItemsFetchCompletedEventHandler OnMenuItemsFetchCompleted;
        public delegate void OnMenuItemsFetchCompletedEventHandler(List<IMenuItem> fetchedMenuItems, int selectedFacadeIndex, GUIFacadeControl.Layout preferredLayout);

        public static PlexItemBase RootItem { get; set; }
        static List<IMenuItem> RootMenu { get; set; }
        static MenuItem ServerItem { get; set; }
        static List<IMenuItem> ServerMenu { get; set; }
        public static IMenuItem CurrentItem { get; set; }
        public static List<string> History { get; set; }


        static Navigation() {
            PlexInterface.ServerManager.OnPlexServersChanged += new ServerManager.OnPlexServersChangedEventHandler(ServerManager_OnPlexServersChanged);
            PlexInterface.OnResponseReceived += new PlexInterface.OnResponseReceivedEventHandler(PlexInterface_OnResponseReceived);
            CommonDialogs.OnProgressCancelled += new CommonDialogs.OnProgressCancelledEventHandler(CommonDialogs_OnProgressCancelled);
            History = new List<string>();
            History.Add(Settings.PLUGIN_NAME);
            RootItem = new PlexItemBase(null, "Root Item", null);
            ServerItem = new MenuItem(RootItem, "Plex Servers");
            RootMenu = new List<IMenuItem>();
            RootMenu.Add(ServerItem);
            RootItem.SetChildItems(RootMenu);
        }

        static void CommonDialogs_OnProgressCancelled() {
            PlexInterface.RequestPlexItemsCancel();
        }


        static void ShowRootMenu(PlexServer selectedServer) {
            CommonDialogs.ShowWaitCursor();
            MediaContainer plexSections = PlexInterface.TryGetPlexSections(selectedServer);
            if (plexSections == null) {
                return;
            }
            RootItem.UriPath = plexSections.UriSource;
            RootMenu = GetCreateSubMenuItems(RootItem, PlexInterface.RequestPlexItems(RootItem.UriPath));
            RootMenu.Add(new PlexItemSearch(RootItem, "Search...", new Uri(PlexInterface.PlexServerCurrent.UriPlexBase, "search?type=0"), "Search Plex Server"));
            RootMenu.Add(ServerItem);
            RootItem.SetChildItems(RootMenu);
            ShowCurrentMenu(RootItem, 0);
            CommonDialogs.HideWaitCursor();
        }

        internal static void CreateStartupMenu(PlexServer lastSelectedOrDefaultServer) {
            CommonDialogs.ShowWaitCursor();
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
            CommonDialogs.HideWaitCursor();
        }

        internal static void ShowCurrentContextMenu() {
            ContextMenu.ShowContextMenu(CurrentItem.Name, CurrentItem.ViewItems);
        }

        internal static void FetchPreviousMenu(IMenuItem currentItem, int storeLastSelectedFacadeIndex) {
            if (currentItem != null && currentItem.Parent != null) {
                History.RemoveAt(History.Count - 1);
                currentItem.LastSelectedChildIndex = storeLastSelectedFacadeIndex;
                ShowCurrentMenu(currentItem.Parent, currentItem.Parent.LastSelectedChildIndex);
            }
        }

        internal static void ShowCurrentMenu(IMenuItem parentItem, int selectFacadeIndex) {
            CommonDialogs.ShowWaitCursor();
            if (parentItem.ChildItems.Count > 0) {
                CurrentItem = parentItem;
                OnMenuItemsFetchCompleted(parentItem.ChildItems, selectFacadeIndex, parentItem.PreferredLayout);
            } else {
                return;
            }
            CommonDialogs.HideWaitCursor();
        }

        internal static List<IMenuItem> GetCreateSubMenuItems(PlexItemBase parentItem, MediaContainer plexResponseConatiner) {            
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
                    dir => new PlexItemSearch(parentItem, dir.prompt, new Uri(parentItem.UriPath, dir.key), dir.prompt))
                    );
                if (!string.IsNullOrEmpty(plexResponseConatiner.viewGroup) && plexResponseConatiner.viewGroup.Equals("secondary")) {
                    parentItem.ViewItems = tmpList;
                }
                tmpList.AddRange(plexResponseConatiner.Video.ConvertAll<IMenuItem>(vid => new PlexItemVideo(parentItem, vid.title, new Uri(parentItem.UriPath, vid.key), vid)));
                tmpList.AddRange(plexResponseConatiner.Track.ConvertAll<IMenuItem>(track => new PlexItemTrack(parentItem, track.title, new Uri(parentItem.UriPath, track.key), track)));

            } catch (Exception e) {
                OnErrorOccured(new PlexException(typeof(Navigation), "Creating submenu failed!", e));
            }
            return tmpList;
        }

        static void PlexInterface_OnResponseReceived(object userToken, MediaContainer response) {
            if (userToken is PlexItemSearch && response.Directory.Count < 1 && response.Video.Count < 1 && response.Track.Count < 1) {
                CommonDialogs.ShowNotifyDialog(10, "Plex Search", "Nothing found...");
            }
            if (userToken is PlexItemBase) {
                var item = userToken as PlexItemBase;
                item.SetChildItems(GetCreateSubMenuItems(item, response));
                History.Add(item.Name);
                ShowCurrentMenu(item, item.LastSelectedChildIndex);
            } else {
                OnErrorOccured(new PlexException(typeof(Navigation), "Unexpected item type in received response!", new InvalidCastException()));
            }          
        }

        private static void ServerManager_OnPlexServersChanged(List<PlexServer> updatedServerList) {
            CommonDialogs.ShowWaitCursor();
            updatedServerList.ForEach(svr => PlexInterface.Login(svr));
            CommonDialogs.HideWaitCursor();
            ServerMenu = updatedServerList.ConvertAll<IMenuItem>(svr => new ActionItem(ServerItem, String.Format("{0} @ {1}", svr.FriendlyName ?? svr.HostName, svr.HostAdress), svr.IsOnline ? Settings.PLEX_ICON_DEFAULT_ONLINE : Settings.PLEX_ICON_DEFAULT_OFFLINE, () => ShowRootMenu(svr)));
            ServerMenu.Add(new ActionItem(null, "Refresh Bonjouor...", Settings.PLEX_ICON_DEFAULT_BONJOUR, () => RefreshServerMenu()));
            ServerMenu.Add(new ActionItem(null, "Add Plex Server...", Settings.PLEX_ICON_DEFAULT_ONLINE, () => AddNewPlexServer()));
            ServerItem.SetChildItems(ServerMenu);
            if (CurrentItem == ServerItem) {
                ShowCurrentMenu(ServerItem, 0);
            }            
        }

        internal static void RefreshServerMenu() {
            PlexInterface.RefreshBonjourServers();
        }

        internal static void AddNewPlexServer() {
            PlexServer newServer = new PlexServer();
            newServer.HostName = CommonDialogs.GetKeyBoardInput("Friendly name", "HostName");
            newServer.HostAdress = CommonDialogs.GetKeyBoardInput("Host Adress", "HostAdress");
            newServer.UserName = CommonDialogs.GetKeyBoardInput("Login", "UserName");
            newServer.UserPass = CommonDialogs.GetKeyBoardInput("Password", "UserPasse");
            if (PlexInterface.Login(newServer)) {
                PlexInterface.ServerManager.SetCurrentPlexServer(newServer);
            } else {
                if (CommonDialogs.ShowCustomYesNo("PlexServer not found!", "The new PlexServer appears to be offline \nor was misconfigured...", "Try Again!", "Cancel", false)) {
                    AddNewPlexServer();
                }
            }
        }

        internal static void RequestChildItems(Uri UriPath, IMenuItem destObject) {
            OnMenuItemsFetchStarted(destObject);
            PlexInterface.RequestPlexItemsAsync(UriPath, destObject);
        }
    }
}

