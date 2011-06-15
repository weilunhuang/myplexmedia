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

namespace MyPlexMedia.Plugin.Window {
    public static class Navigation {
        public static bool IsFetching { get; set; }

        public static event OnErrorOccuredEventHandler OnErrorOccured;
        public delegate void OnErrorOccuredEventHandler(Exception e);
        public static event OnMenuItemsFetchedEventHandler OnMenuItemsFetched;
        public delegate void OnMenuItemsFetchedEventHandler(List<IMenuItem> fetchedMenuItems, int selectedFacadeIndex);

        public static PlexItem RootItem { get; set; }
        static List<IMenuItem> RootMenu { get; set; }
        static MenuItem ServerItem { get; set; }
        static List<IMenuItem> ServerMenu { get; set; }
        public static IMenuItem CurrentItem { get; set; }


        static Navigation() {
            PlexInterface.OnPlexError += new PlexInterface.OnPlexErrorEventHandler(PlexInterface_OnPlexError);
            ServerManager.OnPlexServersChanged += new ServerManager.OnPlexServersChangedEventHandler(ServerManager_OnPlexServersChanged);
            RootItem = new PlexItem(null, "Root Item", null);
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
            RootMenu = GetSubMenuItems(RootItem, plexSections);
            RootMenu.Add(ServerItem);
            RootItem.SetChildItems(RootMenu);
            ShowCurrentMenu(RootItem, 0);
        }

        static void ServerManager_OnPlexServersChanged(List<PlexServer> updatedServerList) {
            ServerMenu = updatedServerList.ConvertAll<IMenuItem>(svr => new ActionItem(ServerItem, svr.HostAdress, svr.IsOnline ? Settings.PLEX_ICON_DEFAULT_ONLINE : Settings.PLEX_ICON_DEFAULT_OFFLINE, () => ShowRootMenu(PlexInterface.TryGetPlexSections(svr))));
            ServerMenu.Add(new ActionItem(null, "Refresh Bonjouor...", Settings.PLEX_ICON_DEFAULT_BONJOUR, () => PlexInterface.RefreshBonjourServers()));
            ServerItem.SetChildItems(ServerMenu);
            if (CurrentItem == ServerItem) {
                ShowCurrentMenu(ServerItem, 0);
            }
        }

        internal static bool CreateStartupMenu() {
            RefreshServerMenu();
            try {
                ShowRootMenu(PlexInterface.TryGetPlexSections());
            } catch (Exception e) {
                if (PlexInterface.PlexServersAvailable) {
                    ShowCurrentMenu(ServerItem, 0);
                } else {

                }
            }
            return true;
        }

        internal static void FetchPreviousMenu(IMenuItem currentItem) {
            if (currentItem != null && currentItem.Parent != null) {
                try {
                    (currentItem.Parent as PlexItem).SetPreferredLayout();
                } catch {
                }
                ShowCurrentMenu(currentItem.Parent, currentItem.Parent.LastSelectedChildIndex);
            } 
        }

        internal static void ShowCurrentMenu(IMenuItem parentItem, int selectFacadeIndex) {
            if (parentItem.ChildItems.Count > 0) {
                CurrentItem = parentItem;
                OnMenuItemsFetched(parentItem.ChildItems, selectFacadeIndex);
            } else {
                return;
            }
        }

        internal static List<IMenuItem> GetSubMenuItems(PlexItem parentItem, MediaContainer plexResponseConatiner) {
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
            return tmpList;
        }

        internal static void RefreshServerMenu() {
            PlexInterface.RefreshBonjourServers();
        }

    }
}

