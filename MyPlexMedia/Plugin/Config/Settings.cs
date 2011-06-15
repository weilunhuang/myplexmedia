using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MediaPortal.GUI.Library;

namespace MyPlexMedia.Plugin.Config {
    public static class Settings {

        public const string PLUGIN_NAME = "MyPlexMedia";
        public const string PLUGIN_AUTHOR = "Anthrax";
        public const string PLUGIN_DESCRIPTION = "A MediaPortal plugin that allows you to browse Plex Media Center.";
        
        public const int PLUGIN_WINDOW_ID = 20110614;
        public static string SKIN_FOLDER_MEDIA = Path.Combine(GUIGraphicsContext.Skin, @"Media\" + PLUGIN_NAME);
        public static string PLUGIN_MEDIA_HOVER = @"hover_MyPlexMedia.png";
        public static string PLEX_ICON_DEFAULT = Path.Combine(SKIN_FOLDER_MEDIA, "icon_default.jpg");
        public static string PLEX_ICON_DEFAULT_BONJOUR = Path.Combine(SKIN_FOLDER_MEDIA, "icon_bonjour.jpg");
        public static string PLEX_ICON_DEFAULT_BACK = Path.Combine(SKIN_FOLDER_MEDIA, "icon_back.jpg");
        public static string PLEX_ICON_DEFAULT_ONLINE = Path.Combine(SKIN_FOLDER_MEDIA, "icon_online.jpg");
        public static string PLEX_ICON_DEFAULT_OFFLINE = Path.Combine(SKIN_FOLDER_MEDIA, "icon_offline.jpg");
        public static string PLEX_SERVER_LIST_XML = @"C:\Program Files (x86)\Team MediaPortal\MediaPortal\plugins\Windows\PlexServers.xml";
        public static string PLEX_ARTWORK_DEFAULT = Path.Combine(SKIN_FOLDER_MEDIA, "fanart_default.jpg");
        public static string PLEX_ARTWORK_CACHE_ROOT_PATH = Path.Combine(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Thumbs), PLUGIN_NAME);
        public static string SKINFILE_MAIN_WINDOW = GUIGraphicsContext.Skin + @"\MyPlexMedia.xml";
        public static string PLEX_ICON_DEFAULT_SEARCH;
        
        public static Dictionary<string, GUIFacadeControl.Layout> PreferredLayouts { get; private set; }
        public static GUIFacadeControl.Layout DefaultLayout { get; private set; }

        static Settings() {
            CreatePreferredLayouts();

            DefaultLayout = GUIFacadeControl.Layout.CoverFlow;
            
            //Set defaults
            FetchCount = 10;
            CheezRootFolder = Path.Combine(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Thumbs), PLUGIN_NAME);
            DeleteLocalCheezOnExit = false;

        }

        private static void CreatePreferredLayouts() {
            PreferredLayouts = new Dictionary<string, GUIFacadeControl.Layout>();
            PreferredLayouts.Add("default", GUIFacadeControl.Layout.CoverFlow);
            PreferredLayouts.Add("secondary", GUIFacadeControl.Layout.List);
            PreferredLayouts.Add("artist", GUIFacadeControl.Layout.LargeIcons);
            PreferredLayouts.Add("album", GUIFacadeControl.Layout.AlbumView);
            PreferredLayouts.Add("show", GUIFacadeControl.Layout.CoverFlow);
            PreferredLayouts.Add("season", GUIFacadeControl.Layout.Filmstrip);
            PreferredLayouts.Add("episode", GUIFacadeControl.Layout.AlbumView);
            PreferredLayouts.Add("track", GUIFacadeControl.Layout.Playlist);
            PreferredLayouts.Add("movie", GUIFacadeControl.Layout.CoverFlow);
        }

        public static int FetchCount { get; set; }
        public static string CheezRootFolder { get; set; }
        public static bool DeleteLocalCheezOnExit { get; set; }

        /// <summary>
        /// Load the settings from the mediaportal config
        /// </summary>
        public static void Load() {
            using (MediaPortal.Profile.Settings reader = new MediaPortal.Profile.Settings(MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml"))) {
                if (!String.IsNullOrEmpty(reader.GetValue(PLUGIN_NAME, "CheezRootFolder"))) {
                    CheezRootFolder = reader.GetValue(PLUGIN_NAME, "CheezRootFolder");
                }                
                FetchCount = reader.GetValueAsInt(PLUGIN_NAME, "FetchCount", FetchCount);             
                DeleteLocalCheezOnExit = reader.GetValueAsBool(PLUGIN_NAME, "DeleteLocalCheezOnExit", DeleteLocalCheezOnExit);                
            }
        }

        /// <summary>
        /// Save the settings to the MP config
        /// </summary>
        public static void Save() {
            using (MediaPortal.Profile.Settings xmlwriter = new MediaPortal.Profile.Settings(MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml"))) {
                xmlwriter.SetValue(PLUGIN_NAME, "CheezRootFolder", CheezRootFolder);
                xmlwriter.SetValue(PLUGIN_NAME, "FetchCount", (int)FetchCount);
                xmlwriter.SetValueAsBool(PLUGIN_NAME, "DeleteLocalCheezOnExit", DeleteLocalCheezOnExit);
            }
        }

        
    }
}
