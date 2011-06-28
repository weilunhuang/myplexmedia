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
using System.IO;
using MediaPortal.GUI.Library;

namespace MyPlexMedia.Plugin.Config {
    public static class Settings {
        public const string PLUGIN_NAME = "MyPlexMedia";
        public const string PLUGIN_AUTHOR = "Anthrax";
        public const string PLUGIN_VERSION = "0.5.0 (Preview)";
        public const string PLUGIN_DESCRIPTION = "A MediaPortal plugin to browse your Plex Media Server(s).";

        public const int PLUGIN_WINDOW_ID = 20110614;
        public static string SKIN_FOLDER_MEDIA = Path.Combine(GUIGraphicsContext.Skin, @"Media\" + PLUGIN_NAME);
        public static string PLUGIN_MEDIA_HOVER = @"hover_MyPlexMedia.png";
        public static string PLEX_ICON_DEFAULT = Path.Combine(SKIN_FOLDER_MEDIA, "icon_default.png");
        public static string PLEX_ICON_DEFAULT_BONJOUR = Path.Combine(SKIN_FOLDER_MEDIA, "icon_bonjour.png");
        public static string PLEX_ICON_DEFAULT_BACK = Path.Combine(SKIN_FOLDER_MEDIA, "icon_back.png");
        public static string PLEX_ICON_DEFAULT_ONLINE = Path.Combine(SKIN_FOLDER_MEDIA, "icon_online.png");
        public static string PLEX_ICON_DEFAULT_OFFLINE = Path.Combine(SKIN_FOLDER_MEDIA, "icon_offline.png");

        public static string PLEX_SERVER_LIST_XML =
            Path.Combine(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config),
                         "PlexServers.xml");

        public static string PLEX_ARTWORK_DEFAULT = Path.Combine(SKIN_FOLDER_MEDIA, "default_fanart.png");

        public static string PLEX_ARTWORK_CACHE_ROOT_PATH =
            Path.Combine(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Thumbs),
                         PLUGIN_NAME);

        public static string SKINFILE_MAIN_WINDOW = GUIGraphicsContext.Skin + @"\MyPlexMedia.xml";
        public static string PLEX_ICON_DEFAULT_SEARCH = Path.Combine(SKIN_FOLDER_MEDIA, "icon_online.png");

        static Settings() {
            DefaultLayout = CreatePreferredLayouts();
            //Set defaults           
            CacheFolder = PLEX_ARTWORK_CACHE_ROOT_PATH;
            DeleteCacheOnExit = false;
        }

        public static Dictionary<string, GUIFacadeControl.Layout> PreferredLayouts { get; private set; }
        public static GUIFacadeControl.Layout DefaultLayout { get; private set; }

        public static PlexMediaCenter.Plex.Connection.PlexServer LastPlexServer { get; set; }

        public static int FetchCount { get; set; }
        public static string CacheFolder { get; set; }
        public static bool DeleteCacheOnExit { get; set; }

        private static GUIFacadeControl.Layout CreatePreferredLayouts() {
            PreferredLayouts = new Dictionary<string, GUIFacadeControl.Layout>
                                   {
                                       {"default", GUIFacadeControl.Layout.List},
                                       {"secondary", GUIFacadeControl.Layout.List},
                                       {"artist", GUIFacadeControl.Layout.LargeIcons},
                                       {"album", GUIFacadeControl.Layout.Filmstrip},
                                       {"show", GUIFacadeControl.Layout.CoverFlow},
                                       {"season", GUIFacadeControl.Layout.CoverFlow},
                                       {"episode", GUIFacadeControl.Layout.List},
                                       {"track", GUIFacadeControl.Layout.Playlist},
                                       {"movie", GUIFacadeControl.Layout.CoverFlow}
                                   };
            //return default Layout
            return GUIFacadeControl.Layout.List;
        }

        public static GUIFacadeControl.Layout GetPreferredLayout(string viewGroup) {
            if (!String.IsNullOrEmpty(viewGroup) && PreferredLayouts.ContainsKey(viewGroup)) {
                return PreferredLayouts[viewGroup];
            }
            return DefaultLayout;
        }

        /// <summary>
        ///   Load the settings from the mediaportal config
        /// </summary>
        public static void Load() {
            using (
                MediaPortal.Profile.Settings reader =
                    new MediaPortal.Profile.Settings(
                        MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config,
                                                                 "MediaPortal.xml"))) {
                if (!String.IsNullOrEmpty(reader.GetValue(PLUGIN_NAME, "CacheFolder"))) {
                    CacheFolder = reader.GetValue(PLUGIN_NAME, "CacheFolder");
                }
                DeleteCacheOnExit = reader.GetValueAsBool(PLUGIN_NAME, "DeleteCacheOnExit", DeleteCacheOnExit);
            }
        }

        /// <summary>
        ///   Save the settings to the MP config
        /// </summary>
        public static void Save() {
            using (
                MediaPortal.Profile.Settings xmlwriter =
                    new MediaPortal.Profile.Settings(
                        MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config,
                                                                 "MediaPortal.xml"))) {
                xmlwriter.SetValue(PLUGIN_NAME, "CacheFolder", CacheFolder);
                xmlwriter.SetValueAsBool(PLUGIN_NAME, "DeleteCacheOnExit", DeleteCacheOnExit);
            }
        }
    }
}