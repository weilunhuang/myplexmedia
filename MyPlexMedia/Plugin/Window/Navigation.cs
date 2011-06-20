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

namespace MyPlexMedia.Plugin.Window {
    public static class Navigation {
        public static bool IsFetching { get; set; }

        public static event OnErrorOccuredEventHandler OnErrorOccured;
        public delegate void OnErrorOccuredEventHandler(Exception e);

        public static event OnMenuItemsFetchStartedEventHandler OnMenuItemsFetchStarted;
        public delegate void OnMenuItemsFetchStartedEventHandler();

        public static event OnMenuItemsFetchCompletedEventHandler OnMenuItemsFetchCompleted;
        public delegate void OnMenuItemsFetchCompletedEventHandler(List<IMenuItem> fetchedMenuItems, int selectedFacadeIndex, GUIFacadeControl.Layout preferredLayout);

        public static PlexItemBase RootItem { get; set; }
        static List<IMenuItem> RootMenu { get; set; }
        static MenuItem ServerItem { get; set; }
        static List<IMenuItem> ServerMenu { get; set; }
        public static IMenuItem CurrentItem { get; set; }


        static Navigation() {
            PlexInterface.ServerManager.OnPlexServersChanged += new ServerManager.OnPlexServersChangedEventHandler(ServerManager_OnPlexServersChanged);
            RootItem = new PlexItemBase(null, "Root Item", null);
            ServerItem = new MenuItem(RootItem, "Plex Servers");
            RootMenu = new List<IMenuItem>();
            RootMenu.Add(ServerItem);
            RootItem.SetChildItems(RootMenu);
        }

        static void ShowRootMenu(MediaContainer plexSections) {
            RootItem.UriPath = plexSections.UriSource;
            RootMenu = GetCreateSubMenuItems(RootItem, RootItem.UriPath);
            RootMenu.Add(ServerItem);
            RootItem.SetChildItems(RootMenu);
            ShowCurrentMenu(RootItem, 0);
        }

        internal static void CreateStartupMenu(PlexServer lastSelectedOrDefaultServer) {
            RefreshServerMenu();
            if (lastSelectedOrDefaultServer != null) {
                try {
                    ShowRootMenu(PlexInterface.TryGetPlexSections(lastSelectedOrDefaultServer));
                    return;
                } catch (Exception e) {
                    OnErrorOccured(e);
                }
            }
            ShowCurrentMenu(ServerItem, 0);
        }

        internal static void ShowCurrentContextMenu() {
            ContextMenu.ShowContextMenu(CurrentItem.Name, CurrentItem.ViewItems);
        }

        internal static void FetchPreviousMenu(IMenuItem currentItem) {
            if (currentItem != null && currentItem.Parent != null) {
                ShowCurrentMenu(currentItem.Parent, currentItem.Parent.LastSelectedChildIndex);
            }
        }

        internal static void ShowCurrentMenu(IMenuItem parentItem, int selectFacadeIndex) {
            if (parentItem.ChildItems.Count > 0) {
                CurrentItem = parentItem;
                OnMenuItemsFetchCompleted(parentItem.ChildItems, selectFacadeIndex, parentItem.PreferredLayout);
            } else {
                return;
            }
        }

        internal static List<IMenuItem> GetCreateSubMenuItems(PlexItemBase parentItem, Uri uriPath) {
            OnMenuItemsFetchStarted();
            List<IMenuItem> tmpList = new List<IMenuItem>();
            try {
                MediaContainer plexResponseConatiner = PlexInterface.RequestPlexItems(uriPath);
                if (plexResponseConatiner == null) {
                    throw new ArgumentNullException("plexResponseContainer");
                }
                //set item meta data
                parentItem.SetMetaData(plexResponseConatiner);
                parentItem.PreferredLayout = Settings.GetPreferredLayout(plexResponseConatiner.viewGroup);
                //Let's see inside and decide what to do               
                if (plexResponseConatiner.viewGroup.Equals("secondary")) {
                    //We have a list of view items...
                    parentItem.ViewItems.AddRange(plexResponseConatiner.Directory.Where(
                        dir => String.IsNullOrEmpty(dir.prompt)).Select<MediaContainerDirectory, IMenuItem>(
                        dir => new PlexItemDirectory(parentItem, dir.title, new Uri(parentItem.UriPath, dir.key), dir))
                        );
                    //And a list of search items
                    parentItem.ViewItems.AddRange(plexResponseConatiner.Directory.Where(
                        dir => !String.IsNullOrEmpty(dir.prompt)).Select<MediaContainerDirectory, IMenuItem>(
                        dir => new PlexItemSearch(parentItem, dir.prompt, new Uri(parentItem.UriPath, dir.key), dir.prompt))
                        );
                    //Mimic iPhone behavior and show 'all' view by default
                    return GetCreateSubMenuItems(parentItem, new Uri(uriPath, plexResponseConatiner.Directory.First().key));
                } else {
                    //We have plain old sub items
                    tmpList.AddRange(plexResponseConatiner.Directory.ConvertAll<IMenuItem>(dir => new PlexItemDirectory(parentItem, dir.title, new Uri(parentItem.UriPath, dir.key), dir)));
                    tmpList.AddRange(plexResponseConatiner.Video.ConvertAll<IMenuItem>(vid => new PlexItemVideo(parentItem, vid.title, new Uri(parentItem.UriPath, vid.key), vid)));
                    tmpList.AddRange(plexResponseConatiner.Track.ConvertAll<IMenuItem>(track => new PlexItemTrack(parentItem, track.title, new Uri(parentItem.UriPath, track.key), track)));
                }
            } catch (Exception e) {
                OnErrorOccured(e);
            }
            return tmpList;
        }

        private static void ServerManager_OnPlexServersChanged(List<PlexServer> updatedServerList) {
            GUIWaitCursor.Init();
            GUIWaitCursor.Show();
            updatedServerList.ForEach(svr => PlexInterface.Login(svr));
            GUIWaitCursor.Hide();
            ServerMenu = updatedServerList.ConvertAll<IMenuItem>(svr => new ActionItem(ServerItem, String.Format("{0} @ {1}", svr.FriendlyName ?? svr.HostName, svr.HostAdress), svr.IsOnline ? Settings.PLEX_ICON_DEFAULT_ONLINE : Settings.PLEX_ICON_DEFAULT_OFFLINE, () => ShowRootMenu(PlexInterface.TryGetPlexSections(svr))));
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
    }
}

