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
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;

namespace MyPlexMedia.Plugin.Config {
    [PluginIcons("MyPlexMedia.Resources.img.MyPlexMedia_enabled.png",
        "MyPlexMedia.Resources.img.MyPlexMedia_disabled.png")]
    public class Configuration : ISetupForm {
        #region Plugin Constructor / Initialization

        public Configuration() {
            Log.Debug(Settings.PLUGIN_NAME + " started!");
        }

        #endregion

        // Returns the name of the plugin which is shown in the plugin menu

        #region ISetupForm Members

        public string PluginName() {
            return Settings.PLUGIN_NAME;
        }

        // Returns the description of the plugin is shown in the plugin menu
        public string Description() {
            return Settings.PLUGIN_DESCRIPTION;
        }

        // Returns the author of the plugin which is shown in the plugin menu
        public string Author() {
            return Settings.PLUGIN_AUTHOR;
        }

        // show the setup dialog
        public void ShowPlugin() {
            new ConfigurationForm().ShowDialog();
        }

        // Indicates whether plugin can be enabled/disabled
        public bool CanEnable() {
            return true;
        }

        // Get Windows-ID
        public int GetWindowId() {
            // WindowID of windowplugin belonging to this setup
            // enter your own unique code
            return Settings.PLUGIN_WINDOW_ID;
        }

        // Indicates if plugin is enabled by default;
        public bool DefaultEnabled() {
            return true;
        }

        // indicates if a plugin has it's own setup screen
        public bool HasSetup() {
            return true;
        }

        /// <summary>
        ///   If the plugin should have it's own button on the main menu of Mediaportal then it
        ///   should return true to this method, otherwise if it should not be on home
        ///   it should return false
        /// </summary>
        /// <param name = "strButtonText">text the button should have</param>
        /// <param name = "strButtonImage">image for the button, or empty for default</param>
        /// <param name = "strButtonImageFocus">image for the button, or empty for default</param>
        /// <param name = "strPictureImage">subpicture for the button or empty for none</param>
        /// <returns>true : plugin needs it's own button on home
        ///   false : plugin does not need it's own button on home</returns>
        public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus,
                            out string strPictureImage) {
            strButtonText = PluginName();
            strButtonImage = String.Empty;
            strButtonImageFocus = String.Empty;
            strPictureImage = Settings.PLUGIN_MEDIA_HOVER;
            return true;
        }

        #endregion
    }
}