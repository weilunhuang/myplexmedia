using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyPlexMedia.Plugin.Window.Items;
using System.IO;
using MediaPortal.GUI.Library;
using PlexMediaCenter.Plex.Data.Types;

namespace MyPlexMedia.Plugin.Window {
    public partial class Main {

        void MenuItem_OnHasBackground(string imagePath) {
            if (ctrlBackgroundImage != null && !String.IsNullOrEmpty(imagePath) && File.Exists(imagePath)) {
                ctrlBackgroundImage.SetFileName(imagePath);
            }
        }

        void MediaRetrieval_OnArtWorkRetrieved(string artWork) {
            if (facadeLayout.NeedRefresh()) {
                facadeLayout.DoUpdate();
            }
        }

        void Navigation_OnMenuItemsFetched(List<IMenuItem> fetchedMenuItems) {
            Dialogs.CurrentSearchItems = new List<PlexItemSearch>();
            facadeLayout.Clear();
            foreach (var item in fetchedMenuItems) {
                if (item is PlexItemSearch) {
                    Dialogs.CurrentSearchItems.Add(item as PlexItemSearch);
                    continue;
                }else if (item is MenuItem) {
                    facadeLayout.Add((MenuItem)item);
                }
            }            
            facadeLayout.DoUpdate();
        }

        void MenuItem_OnMenuItemSelected(IMenuItem selectedItem) {
            UpdateGuiProperties(selectedItem);
        }       

        void MenuItem_OnPreferredLayout(GUIFacadeControl.Layout preferredLayout) {
            if (CurrentLayout != preferredLayout) {
                CurrentLayout = preferredLayout;
                SwitchLayout();
            }
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
