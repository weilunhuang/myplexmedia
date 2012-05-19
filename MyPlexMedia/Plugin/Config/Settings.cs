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
using MyPlexMedia.Plugin.Window.Playback;
using MyPlexMedia.Plugin.Window.Dialogs;
using PlexMediaCenter.Plex.Connection;
using MediaPortal.Services;
using NLog;
using MpConf = MediaPortal.Configuration.Config;
using NLog.Targets;
using NLog.Config;
namespace MyPlexMedia.Plugin.Config {
    public static class Settings {
        #region SectionType enum

        public enum SectionType {
            Music,
            Video,
            Photo
        }

        #endregion

        public const string PLUGIN_NAME = "MyPlexMedia";
        public const string PLUGIN_AUTHOR = "Anthrax";
        public const string PLUGIN_VERSION = "1.0.0";
        public const string PLUGIN_DESCRIPTION = "A MediaPortal plugin to browse your Plex Media Server(s).";

        public static string PLUGIN_LOG_FILE = Path.Combine(MpConf.GetFolder(MpConf.Dir.Log), PLUGIN_NAME + ".txt");

        public const int PLUGIN_WINDOW_ID = 20110614;
        public const int DIALOG_BUFFERING_WINDOW_ID = 20110615;

        public static string SKIN_FOLDER_MEDIA = Path.Combine(GUIGraphicsContext.Skin, @"Media\" + PLUGIN_NAME);
        public static string PLUGIN_MEDIA_HOVER = @"hover_MyPlexMedia.png";
        public static string PLEX_BACKGROUND_DEFAULT = Path.Combine(SKIN_FOLDER_MEDIA, "default_background.jpg");
        public static string PLEX_ICON_DEFAULT = Path.Combine(SKIN_FOLDER_MEDIA, "icon_default.png");
        public static string PLEX_ICON_DEFAULT_BONJOUR = Path.Combine(SKIN_FOLDER_MEDIA, "icon_bonjour.png");
        public static string PLEX_ICON_DEFAULT_BACK = Path.Combine(SKIN_FOLDER_MEDIA, "icon_back.png");
        public static string PLEX_ICON_DEFAULT_ONLINE = Path.Combine(SKIN_FOLDER_MEDIA, "icon_online.png");
        public static string PLEX_ICON_DEFAULT_OFFLINE = Path.Combine(SKIN_FOLDER_MEDIA, "icon_offline.png");
        public static string PLEX_ARTWORK_CACHE_ROOT_PATH = Path.Combine(MpConf.GetFolder(MpConf.Dir.Thumbs), PLUGIN_NAME);


        public static string PLEX_SERVER_LIST_XML = Path.Combine(MpConf.GetFolder(MpConf.Dir.Config), "PlexServers.xml");

        public static string PLEX_ARTWORK_DEFAULT = Path.Combine(SKIN_FOLDER_MEDIA, "default_fanart.png");


        public static string SKINFILE_MAIN_WINDOW = GUIGraphicsContext.Skin + @"\MyPlexMedia.xml";

        public static string SKINFILE_DIALOG_BUFFERING = GUIGraphicsContext.Skin + @"\MyPlexMedia.GuiDialogBufferingProgress.xml";

        public static string PLEX_ICON_DEFAULT_SEARCH = Path.Combine(SKIN_FOLDER_MEDIA, "icon_online.png");

        static Settings() {
            InitLogger();
            DefaultLayout = CreatePreferredLayouts();
            //Set defaults           
            MyPlexUser = String.Empty;
            MyPlexPass = String.Empty;
            CacheFolder = PLEX_ARTWORK_CACHE_ROOT_PATH;
            DefaultQualityLAN = Window.Playback.PlexQualities._7_8000kbps_1080p;
            DefaultQualityWAN = Window.Playback.PlexQualities._3_1500kbps_480p;
            SelectQualityPriorToPlayback = true;
            ShowDebugwindow = true;
            DeleteCacheOnExit = false;
            DownloadArtwork = true;
        }

        public static MediaPortal.Profile.Settings MediaPortalSettings {
            get {
                return new MediaPortal.Profile.Settings(MpConf.GetFile(MpConf.Dir.Config, "MediaPortal.xml"));
            }
        }

        private static void InitLogger() {
            // if no configuration exists go ahead and create one
            if (LogManager.Configuration == null) LogManager.Configuration = new LoggingConfiguration();

            // build the logging target for moving pics logging
            FileTarget pluginLogTarget = new FileTarget();
            pluginLogTarget.Name = "moving-pictures";
            pluginLogTarget.FileName = PLUGIN_LOG_FILE;
            pluginLogTarget.Layout = "${date:format=dd-MMM-yyyy HH\\:mm\\:ss} " +
                                "${level:fixedLength=true:padding=5} " +
                                "[${logger:fixedLength=true:padding=20:shortName=true}]: ${message} " +
                                "${exception:format=tostring}";

            LogManager.Configuration.AddTarget("moving-pictures", pluginLogTarget);
            // Get current Log Level from MediaPortal
            LogLevel logLevel = LogLevel.Debug;
            switch ((Level)MediaPortalSettings.GetValueAsInt("general", "loglevel", 0)) {
                case Level.Error:
                    logLevel = LogLevel.Error;
                    break;
                case Level.Warning:
                    logLevel = LogLevel.Warn;
                    break;
                case Level.Information:
                    logLevel = LogLevel.Info;
                    break;
                case Level.Debug:
                default:
                    logLevel = LogLevel.Debug;
                    break;
            }

            // set the logging rules for moving pics logging
            LoggingRule pluginRule = new LoggingRule("MyPlexMedia.*", logLevel, pluginLogTarget);
            LoggingRule plexRule = new LoggingRule("PlexMediaCenter.*", logLevel, pluginLogTarget);
            LogManager.Configuration.LoggingRules.Add(pluginRule);
            LogManager.Configuration.LoggingRules.Add(plexRule);

            // force NLog to reload the configuration data
            LogManager.Configuration = LogManager.Configuration;
        }

        public static Window.Playback.PlexQualities DefaultQualityLAN { get; set; }

        public static Window.Playback.PlexQualities DefaultQualityWAN { get; set; }

        public static Dictionary<string, PlexSectionLayout> PreferredLayouts { get; private set; }
        public static PlexSectionLayout DefaultLayout { get; private set; }

        public static int FetchCount { get; set; }
        public static string CacheFolder { get; set; }
        public static string MyPlexUser { get; set; }
        public static string MyPlexPass { get; set; }
        public static bool DeleteCacheOnExit { get; set; }
        public static bool SelectQualityPriorToPlayback { get; set; }
        public static bool ShowDebugwindow { get; set; }
        public static bool DownloadArtwork { get; set; }

        private static PlexSectionLayout CreatePreferredLayouts() {
            PreferredLayouts = new Dictionary<string, PlexSectionLayout>
                                   {
                                       {
                                           "default",
                                           new PlexSectionLayout
                                               {Layout = GUIFacadeControl.Layout.List, Section = SectionType.Music}
                                           },
                                       {
                                           "secondary",
                                           new PlexSectionLayout
                                               {Layout = GUIFacadeControl.Layout.List, Section = SectionType.Music}
                                           },
                                       {
                                           "artist",
                                           new PlexSectionLayout
                                               {Layout = GUIFacadeControl.Layout.LargeIcons, Section = SectionType.Music }
                                           },
                                       {
                                           "album",
                                           new PlexSectionLayout
                                               {Layout = GUIFacadeControl.Layout.AlbumView, Section = SectionType.Music}
                                           },
                                       {
                                           "show",
                                           new PlexSectionLayout
                                               {Layout = GUIFacadeControl.Layout.CoverFlow, Section = SectionType.Video}
                                           },
                                       {
                                           "season",
                                           new PlexSectionLayout
                                               {Layout = GUIFacadeControl.Layout.CoverFlow, Section = SectionType.Video}
                                           },
                                       {
                                           "episode",
                                           new PlexSectionLayout
                                               {Layout = GUIFacadeControl.Layout.List, Section = SectionType.Photo}
                                           },
                                       {
                                           "track",
                                           new PlexSectionLayout
                                               {Layout = GUIFacadeControl.Layout.Playlist, Section = SectionType.Music}
                                           },
                                       {
                                           "movie",
                                           new PlexSectionLayout
                                               {Layout = GUIFacadeControl.Layout.CoverFlow, Section = SectionType.Video}
                                           },
                                                  {
                                            "photo",
                                            new PlexSectionLayout
                                                {Layout = GUIFacadeControl.Layout.LargeIcons, Section = SectionType.Photo}
                                            },
                                   };
            //return default Layout
            return PreferredLayouts["default"];
        }

        public static PlexSectionLayout GetPreferredLayout(string viewGroup) {
            if (!String.IsNullOrEmpty(viewGroup) && PreferredLayouts.ContainsKey(viewGroup)) {
                return PreferredLayouts[viewGroup];
            }
            return DefaultLayout;
        }

        /// <summary>
        ///   Load the settings from the mediaportal config
        /// </summary>
        public static void Load() {
            try {
                CacheFolder = MediaPortalSettings.GetValueAsString(PLUGIN_NAME, "CacheFolder", CacheFolder);
                DefaultQualityLAN = Enum<PlexQualities>.Parse(MediaPortalSettings.GetValueAsString(PLUGIN_NAME, "DefaultQualityLAN", DefaultQualityLAN.ToString()));
                DefaultQualityWAN = Enum<PlexQualities>.Parse(MediaPortalSettings.GetValueAsString(PLUGIN_NAME, "DefaultQualityWAN", DefaultQualityWAN.ToString()));
                MyPlexUser = MediaPortalSettings.GetValue(PLUGIN_NAME, "MyPlexUser");
                MyPlexPass = decryptString(MediaPortalSettings.GetValue(PLUGIN_NAME, "MyPlexPass"));
                SelectQualityPriorToPlayback = MediaPortalSettings.GetValueAsBool(PLUGIN_NAME, "SelectQualityPriorToPlayback", true);
                DeleteCacheOnExit = MediaPortalSettings.GetValueAsBool(PLUGIN_NAME, "DeleteCacheOnExit", DeleteCacheOnExit);
                ShowDebugwindow = MediaPortalSettings.GetValueAsBool(PLUGIN_NAME, "ShowDebugwindow", ShowDebugwindow);
                DownloadArtwork = MediaPortalSettings.GetValueAsBool(PLUGIN_NAME, "DownloadArtwork", DownloadArtwork);
            } catch { }
        }

        /// <summary>
        ///   Save the settings to the MP config
        /// </summary>
        public static void Save() {
            try {
                MediaPortalSettings.SetValue(PLUGIN_NAME, "CacheFolder", CacheFolder);
                MediaPortalSettings.SetValue(PLUGIN_NAME, "DefaultQualityLAN", DefaultQualityLAN);
                MediaPortalSettings.SetValue(PLUGIN_NAME, "DefaultQualityWAN", DefaultQualityWAN);
                MediaPortalSettings.SetValue(PLUGIN_NAME, "MyPlexUser", MyPlexUser);
                MediaPortalSettings.SetValue(PLUGIN_NAME, "MyPlexPass", encryptString(MyPlexPass));
                MediaPortalSettings.SetValueAsBool(PLUGIN_NAME, "SelectQualityPriorToPlayback", SelectQualityPriorToPlayback);
                MediaPortalSettings.SetValueAsBool(PLUGIN_NAME, "DownloadArtwork", DownloadArtwork);
            } catch { }
        }

        /// <summary>
        /// Decrypt an encrypted setting string
        /// </summary>
        /// <param name="encrypted">The string to decrypt</param>
        /// <returns>The decrypted string or an empty string if something went wrong</returns>
        private static string decryptString(string encrypted) {
            string decrypted = String.Empty;

            EncryptDecrypt Crypto = new EncryptDecrypt();
            try {
                decrypted = Crypto.Decrypt(encrypted);
            } catch (Exception) {
                MediaPortal.GUI.Library.Log.Error("Could not decrypt config string!");
            }

            return decrypted;
        }

        /// <summary>
        /// Encrypt a setting string
        /// </summary>
        /// <param name="decrypted">An unencrypted string</param>
        /// <returns>The string encrypted</returns>
        private static string encryptString(string decrypted) {
            EncryptDecrypt Crypto = new EncryptDecrypt();
            string encrypted = String.Empty;

            try {
                encrypted = Crypto.Encrypt(decrypted);
            } catch (Exception) {
                MediaPortal.GUI.Library.Log.Error("Could not encrypt setting string!");
                encrypted = String.Empty;
            }

            return encrypted;
        }

        #region Nested type: PlexSectionLayout

        public struct PlexSectionLayout {
            public SectionType Section { get; set; }
            public GUIFacadeControl.Layout Layout { get; set; }
        }

        #endregion

    }
}