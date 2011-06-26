using System;
using System.Collections.Generic;
using System.IO;
using MediaPortal.GUI.Library;
using MediaPortal.Util;
using MyPlexMedia.Plugin.Window.Items;
using PlexMediaCenter.Plex.Data.Types;
using MyPlexMedia.Plugin.Window.Dialogs;
using MyPlexMedia.Plugin.Config;
using PlexMediaCenter.Plex;


namespace MyPlexMedia.Plugin.Window {
    public partial class Main {


        void PlexInterface_OnPlexError(PlexException plexError) {
            Log.Error(plexError);
            CommonDialogs.HideProgressDialog();
            CommonDialogs.ShowNotifyDialog(30, plexError.ErrorSource.ToString(), plexError.Message);
        }


        void PlexInterface_OnResponseProgress(object userToken, int progress) {
            CommonDialogs.ShowProgressDialog("Fetching Plex Items...", ((IMenuItem)userToken).Name, progress);
        }

        void MenuItem_OnHasBackground(string imagePath) {
            if (ctrlBackgroundImage != null && !String.IsNullOrEmpty(imagePath) && File.Exists(imagePath) && !ctrlBackgroundImage.ImagePath.Equals(imagePath)) {
                ctrlBackgroundImage.ResetAnimations();
                ctrlBackgroundImage.SetFileName(imagePath);
            }
        }

        void MediaRetrieval_OnArtWorkRetrieved(string artWork) {
            Utils.DoInsertExistingFileIntoCache(artWork);
            if (facadeLayout.SelectedListItem != null) {
                ((IMenuItem)facadeLayout.SelectedListItem).OnSelected();
            }
        }

        void Navigation_OnMenuItemsFetchStarted(IMenuItem itemToFetch) {
            CommonDialogs.HideProgressDialog();
        }

        void Navigation_OnMenuItemsFetchCompleted(List<IMenuItem> fetchedMenuItems, int selectedFacadeIndex, GUIFacadeControl.Layout preferredLayout) {
            CommonDialogs.HideProgressDialog();
            GUIPropertyManager.SetProperty("#currentmodule", String.Join(">", Navigation.History.ToArray()));
            facadeLayout.Clear();
            facadeLayout.ListLayout.Clear();           
            facadeLayout.CoverFlowLayout.Clear();
            facadeLayout.ThumbnailLayout.Clear();
            facadeLayout.FilmstripLayout.Clear();
            facadeLayout.ListLayout.Clear();
            facadeLayout.PlayListLayout.Clear();
            CurrentLayout = preferredLayout;
            SwitchLayout();
           
            foreach (var item in fetchedMenuItems) {
                facadeLayout.Add(item as MenuItem);
            }
            facadeLayout.RefreshCoverArt();
            facadeLayout.SelectedListItemIndex = selectedFacadeIndex;
            facadeLayout.CoverFlowLayout.SelectCard(selectedFacadeIndex);
            //facadeLayout.DoUpdate();                
        }

        void MenuItem_OnMenuItemSelected(IMenuItem selectedItem) {
            UpdateGuiProperties(selectedItem);
        }

        private void UpdateGuiProperties(MediaContainer itemDetails) {
            //TODO: add custom skin properties
        }

        private void UpdateGuiProperties(IMenuItem selectedItem) {
            //TODO: add custom skin properties
        }
    }
}
