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

namespace MyPlexMedia.Plugin.Window {
    public static class Navigation {
        public static bool IsFetching { get; set; }

        public static event OnErrorOccuredEventHandler OnErrorOccured;
        public delegate void OnErrorOccuredEventHandler(Exception e);

        public static event OnMenuItemsFetchStartedEventHandler OnMenuItemsFetchStarted;
        public delegate void OnMenuItemsFetchStartedEventHandler();

        public static event OnMenuItemsFetchCompletedEventHandler OnMenuItemsFetchCompleted;
        public delegate void OnMenuItemsFetchCompletedEventHandler(List<IMenuItem> fetchedMenuItems, int selectedFacadeIndex);

        public static PlexItemBase RootItem { get; set; }
        static List<IMenuItem> RootMenu { get; set; }
        static MenuItem ServerItem { get; set; }
        static List<IMenuItem> ServerMenu { get; set; }
        public static IMenuItem CurrentItem { get; set; }


        static Navigation() {
            PlexInterface.OnPlexError += new PlexInterface.OnPlexErrorEventHandler(PlexInterface_OnPlexError);
            PlexInterface.ServerManager.OnPlexServersChanged += new ServerManager.OnPlexServersChangedEventHandler(ServerManager_OnPlexServersChanged);
            RootItem = new PlexItemBase(null, "Root Item", null);
            ServerItem = new MenuItem(RootItem, "Plex Servers");
            RootMenu = new List<IMenuItem>();
            RootMenu.Add(ServerItem);
            RootItem.SetChildItems(RootMenu);
        }


        static void PlexInterface_OnPlexError(Exception e) {
            OnErrorOccured(e);
        }

        static void ShowRootMenu(MediaContainer plexSections) {
            RootItem.UriPath = plexSections.UriSource;
            RootMenu = GetSubMenuItems(RootItem, RootItem.UriPath);
            RootMenu.Add(ServerItem);
            RootItem.SetChildItems(RootMenu);
            ShowCurrentMenu(RootItem, 0);
        }

        static void ServerManager_OnPlexServersChanged(List<PlexServer> updatedServerList) {
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

        internal static void FetchPreviousMenu(IMenuItem currentItem) {
            if (currentItem != null && currentItem.Parent != null) {
                try {
                    (currentItem.Parent as PlexItemBase).SetPreferredLayout();
                } catch {
                }
                ShowCurrentMenu(currentItem.Parent, currentItem.Parent.LastSelectedChildIndex);
            }
        }

        internal static void ShowCurrentMenu(IMenuItem parentItem, int selectFacadeIndex) {
            if (parentItem.ChildItems.Count > 0) {
                CurrentItem = parentItem;
                OnMenuItemsFetchCompleted(parentItem.ChildItems, selectFacadeIndex);
            } else {
                return;
            }
        }

        internal static List<IMenuItem> GetSubMenuItems(PlexItemBase parentItem, Uri uriPath) {
            OnMenuItemsFetchStarted();
            MediaContainer plexResponseConatiner = PlexInterface.RequestPlexItems(uriPath);
            if (!String.IsNullOrEmpty(plexResponseConatiner.viewGroup) && Settings.PreferredLayouts.ContainsKey(plexResponseConatiner.viewGroup)) {
                parentItem.PreferredLayout = Settings.PreferredLayouts[plexResponseConatiner.viewGroup];
            } else {
                parentItem.PreferredLayout = Settings.DefaultLayout;
            }
            parentItem.SetItemInfos(plexResponseConatiner);
            //Add ActionItems
            List<IMenuItem> tmpList = new List<IMenuItem>();
            tmpList.AddRange(plexResponseConatiner.Directory.ConvertAll<IMenuItem>(dir => String.IsNullOrEmpty(dir.prompt) ? (IMenuItem)new PlexItemDirectory(parentItem, dir.title, new Uri(parentItem.UriPath, dir.key), dir) : (IMenuItem)new PlexItemSearch(parentItem, dir.prompt, new Uri(parentItem.UriPath, dir.key), dir.prompt)));
            tmpList.AddRange(plexResponseConatiner.Video.ConvertAll<IMenuItem>(vid => new PlexItemVideo(parentItem, vid.title, new Uri(parentItem.UriPath, vid.key), vid)));
            tmpList.AddRange(plexResponseConatiner.Track.ConvertAll<IMenuItem>(track => new PlexItemTrack(parentItem, track.title, new Uri(parentItem.UriPath, track.key), track)));
            return tmpList;
        }

        internal static void RefreshServerMenu() {
            PlexInterface.RefreshBonjourServers();
        }

        internal static void AddNewPlexServer() {
            PlexServer newServer = new PlexServer();
            newServer.HostName = Dialogs.GetKeyBoardInput("Friendly name", "HostName");
            newServer.HostAdress = Dialogs.GetKeyBoardInput("Host Adress", "HostAdress");
            newServer.UserName = Dialogs.GetKeyBoardInput("Login", "UserName");
            newServer.UserPass = Dialogs.GetKeyBoardInput("Password", "UserPasse");
            if (PlexInterface.Login(newServer)) {
                PlexInterface.ServerManager.SetCurrentPlexServer(newServer);
            } else {
                if (Dialogs.ShowCustomYesNo("PlexServer not found!", "The new PlexServer appears to be offline \nor was misconfigured...", "Try Again!", "Cancel", false)) {
                    AddNewPlexServer();
                }
            }
        }

    }
}

