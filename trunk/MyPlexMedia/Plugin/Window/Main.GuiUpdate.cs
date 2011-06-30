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
            CommonDialogs.ShowNotifyDialog(30, plexError.ErrorSource.ToString(), plexError.Message);
        }

        private static void PlexInterface_OnResponseProgress(object userToken, int progress) {
            CommonDialogs.ShowProgressDialog(progress, "Fetching Plex Items...", ((IMenuItem) userToken).Name);
        }

        private void MenuItem_OnHasBackground(string imagePath) {
            if (ctrlBackgroundImage == null || String.IsNullOrEmpty(imagePath) || !File.Exists(imagePath) ||
                ctrlBackgroundImage.ImagePath.Equals(imagePath)) return;
            ctrlBackgroundImage.RemoveMemoryImageTexture();
            ctrlBackgroundImage.SetFileName(imagePath);
            ctrlBackgroundImage.DoUpdate();
            GUIWindowManager.Process();
        }

        private static void Navigation_OnMenuItemsFetchStarted(IMenuItem itemToFetch) {
            CommonDialogs.HideProgressDialog();
        }

        private void Navigation_OnMenuItemsFetchCompleted(List<IMenuItem> fetchedMenuItems, int selectedFacadeIndex,
                                                          Settings.PlexSectionLayout preferredLayout) {
            CommonDialogs.HideProgressDialog();
            GUIPropertyManager.SetProperty("#currentmodule", String.Join(">", Navigation.History.ToArray()));
            facadeLayout.Clear();
            facadeLayout.ListLayout.Clear();
            facadeLayout.CoverFlowLayout.Clear();
            facadeLayout.ThumbnailLayout.Clear();
            facadeLayout.FilmstripLayout.Clear();
            facadeLayout.ListLayout.Clear();
            facadeLayout.PlayListLayout.Clear();
            CurrentLayout = preferredLayout.Layout;
            SwitchLayout();

            foreach (var item in fetchedMenuItems) {
                facadeLayout.Add(item as MenuItem);
            }
            facadeLayout.RefreshCoverArt();
            facadeLayout.SelectedListItemIndex = selectedFacadeIndex;
            facadeLayout.CoverFlowLayout.SelectCard(selectedFacadeIndex);
            //facadeLayout.DoUpdate();                
        }

        private static void MenuItem_OnMenuItemSelected(IMenuItem selectedItem) {
            UpdateGuiProperties(selectedItem);
        }

        private static void UpdateGuiProperties(IMenuItem selectedItem) {
            //TODO: add custom skin properties
        }

        private static void PlexVideoPlayer_OnPlexVideoPlayBack(MediaContainerVideo nowPlaying) {
            GUIPropertyManager.SetProperty("#Play.Current.Title", nowPlaying.title);
            GUIPropertyManager.SetProperty("#Play.Current.File", nowPlaying.Media[0].Part[0].file);
            GUIPropertyManager.SetProperty("#Play.Current.Thumb", nowPlaying.thumb);
            GUIPropertyManager.SetProperty("#Play.Current.Plot", nowPlaying.summary);
            GUIPropertyManager.SetProperty("#Play.Current.PlotOutline", nowPlaying.tagline);
            GUIPropertyManager.SetProperty("#Play.Current.Rating", nowPlaying.rating);
            GUIPropertyManager.SetProperty("#Play.Current.MPAARating", nowPlaying.contentRating);
            GUIPropertyManager.SetProperty("#Play.Current.Year", nowPlaying.year);
            GUIPropertyManager.SetProperty("#Play.Current.Runtime", nowPlaying.duration);
            GUIPropertyManager.SetProperty("#Play.Current.AspectRatio", nowPlaying.Media[0].aspectRatio);
            GUIPropertyManager.SetProperty("#Play.Current.VideoResolution", nowPlaying.Media[0].videoResolution);
            GUIPropertyManager.SetProperty("#Play.Current.VideoCodec.Texture", nowPlaying.Media[0].videoCodec);
            GUIPropertyManager.SetProperty("#Play.Current.AudioCodec.Texture", nowPlaying.Media[0].audioCodec);
            GUIPropertyManager.SetProperty("#Play.Current.AudioChannels", nowPlaying.Media[0].audioChannels);
        }
    }
}