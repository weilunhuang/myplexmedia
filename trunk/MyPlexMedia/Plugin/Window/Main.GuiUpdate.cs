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
using System.IO;
using MediaPortal.GUI.Library;
using MediaPortal.Util;
using MyPlexMedia.Plugin.Config;
using MyPlexMedia.Plugin.Window.Dialogs;
using MyPlexMedia.Plugin.Window.Items;
using PlexMediaCenter.Plex;
using PlexMediaCenter.Plex.Data.Types;

namespace MyPlexMedia.Plugin.Window {
    public partial class Main {
        private static void PlexInterface_OnPlexError(PlexException plexError) {
            Log.Error(plexError);
            CommonDialogs.HideProgressDialog();
            CommonDialogs.ShowNotifyDialog(30, Settings.PLUGIN_NAME + " Error!", plexError.ErrorSource.ToString() + ": " + plexError.Message, Settings.PLEX_ICON_DEFAULT_OFFLINE, CommonDialogs.PLUGIN_NOTIFY_WINDOWS.WINDOW_DIALOG_AUTO);
        }

        private static void PlexInterface_OnResponseProgress(object userToken, int progress) {
            if (progress < 1 || progress > 99) {
                return;
            }
            CommonDialogs.ShowProgressDialog(progress, Settings.PLUGIN_NAME, "Fetching Plex Items...",
                                             ((IMenuItem)userToken).Parent.Name + " > " + ((IMenuItem)userToken).Name,
                                             String.Format("Current Progress: {0,3}%", progress.ToString()));

        }

        private void SetBackgroundImage(string imagePath) {
            if (ctrlBackgroundImage == null || ctrlBackgroundImage.FileName.Equals(imagePath)) {
                return;
            }
            if (!String.IsNullOrEmpty(imagePath) && File.Exists(imagePath)) {
                //GUITextureManager.ReleaseTexture(ctrlBackgroundImage.FileName);
                //ctrlBackgroundImage.RemoveMemoryImageTexture();
                if (GUITextureManager.Load(imagePath, 0, 0, 0, true) > 0) {
                    ctrlBackgroundImage.SetFileName(imagePath);
                }
            }
            //ctrlBackgroundImage.RemoveMemoryImageTexture();
            ctrlBackgroundImage.BringIntoView();
            ctrlBackgroundImage.DoUpdate();
            ctrlBackgroundImage.Refresh();
            GUIWindowManager.Process();
        }

        private static void Navigation_OnMenuItemsFetchStarted(IMenuItem itemToFetch) {
            CommonDialogs.ShowWaitCursor();
        }

        private void Navigation_OnMenuItemsFetchCompleted(IMenuItem parentItem, int selectedFacadeIndex) {
            if (parentItem.ChildItems == null || parentItem.ChildItems.Count < 1) {
                return;
            }
            GUIPropertyManager.SetProperty("#currentmodule", GetHistory(parentItem));
            CurrentLayout = parentItem.PreferredLayout.Layout;
            SwitchLayout();
            facadeLayout.Clear();
            parentItem.ChildItems.ForEach(item => facadeLayout.Add(item as MenuItem));
            facadeLayout.SelectedListItemIndex = selectedFacadeIndex;
            facadeLayout.CoverFlowLayout.SelectCard(selectedFacadeIndex);
            CommonDialogs.HideWaitCursor();
            CommonDialogs.HideProgressDialog();
            SetBackgroundImage(parentItem.ChildItems[selectedFacadeIndex].BackgroundImage);
        }

        private void MenuItem_OnMenuItemSelected(IMenuItem selectedItem) {
            SetBackgroundImage(selectedItem.BackgroundImage);
        }

        private string GetHistory(IMenuItem current, string concat = "", int level = 0) {
            if (level < 2 && current.Parent != null) {
                concat = String.Format("{0}>", GetHistory(current.Parent, concat, ++level));
            }
            return concat + current.Name;
        }
    }
}