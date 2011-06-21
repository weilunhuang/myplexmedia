using System;
using System.Collections.Generic;
using System.IO;
using MediaPortal.GUI.Library;
using MediaPortal.Util;
using MyPlexMedia.Plugin.Window.Items;
using PlexMediaCenter.Plex.Data.Types;
using MyPlexMedia.Plugin.Window.Dialogs;


namespace MyPlexMedia.Plugin.Window {
    public partial class Main {



        void PlexInterface_OnResponseProgress(object userToken, int progress) {
            GUIPropertyManager.SetProperty("#fanarthandler.scraper.percent.completed", progress.ToString() + "%");
            CommonDialogs.UpdateProgressDialog(((IMenuItem)userToken).Name, progress);
        }      

        void MenuItem_OnHasBackground(string imagePath) {
            if (ctrlBackgroundImage != null && !String.IsNullOrEmpty(imagePath) && File.Exists(imagePath)) {
                ctrlBackgroundImage.SetFileName(imagePath);
            }
        }

        void MediaRetrieval_OnArtWorkRetrieved(string artWork) {
            Utils.DoInsertExistingFileIntoCache(artWork);
            if (facadeLayout.SelectedListItem != null) {
                ((IMenuItem)facadeLayout.SelectedListItem).OnSelected();
            }
        }

        void Navigation_OnMenuItemsFetchStarted() {
            GUIWaitCursor.Init();
            GUIWaitCursor.Show();
        }

        void Navigation_OnMenuItemsFetchCompleted(List<IMenuItem> fetchedMenuItems, int selectedFacadeIndex, GUIFacadeControl.Layout preferredLayout) {
            GUIWaitCursor.Hide();

            CurrentLayout = preferredLayout;
            SwitchLayout();
            facadeLayout.Clear();
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
       
        void PlexItem_OnItemDetailsUpdated(MediaContainer itemDetails) {
            UpdateGuiProperties(itemDetails);
        }

        private void UpdateGuiProperties(MediaContainer itemDetails) {
            //TODO: add custom skin properties
        }

        private void UpdateGuiProperties(IMenuItem selectedItem) {
            //TODO: add custom skin properties
        }
    }
}
