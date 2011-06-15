using System;
using System.Collections.Generic;
using System.Threading;
using MediaPortal.GUI.Library;
using MyPlexMedia.Plugin.Config;
using MyPlexMedia.Plugin.Window.Items;
using WindowPlugins;
using PlexMediaCenter.Plex;
using PlexMediaCenter.Util;
using System.IO;
using MediaPortal.GUI.Video;

namespace MyPlexMedia.Plugin.Window {
    public partial class Main : WindowPluginBase {

        public Main() {
            GetID = Settings.PLUGIN_WINDOW_ID;
        }

        #region Private Members

        private List<IMenuItem> CurrentMenuItems { get; set; }

        #endregion

        #region Enums

        #endregion

        #region Skin Controls

        [SkinControlAttribute(2011)]
        protected GUIImage ctrlBackgroundImage = null;

        #endregion

        #region GUIWindow Base Class Overrides

        protected override string SerializeName {
            get {
                return Settings.PLUGIN_NAME;
            }
        }

        public override bool Init() {
            LoadSettings();
            GUIPropertyManager.SetProperty("#currentmodule", Settings.PLUGIN_NAME);
            PlexInterface.Init(Settings.PLEX_SERVER_LIST_XML, Settings.PLEX_ARTWORK_CACHE_ROOT_PATH, Settings.PLEX_ICON_DEFAULT);
            return Load(Settings.SKINFILE_MAIN_WINDOW);            
        }

        public override void DeInit() {
            base.DeInit();
        }

        protected override void LoadSettings() {
            Settings.Load();
        }

        protected override void OnPageLoad() {
            base.OnPageLoad();
            RegisterEventHandlers();
            Navigation.CreateStartupMenu();
            CurrentLayout = Settings.DefaultLayout;
            SwitchLayout();
        }



        protected override void OnPageDestroy(int new_windowId) {
            UnRegisterEventHandlers();
            base.OnPageDestroy(new_windowId);
        }

        protected override void OnShowViews() {

        }

        protected override bool AllowLayout(GUIFacadeControl.Layout layout) {
            switch (layout) {
                case GUIFacadeControl.Layout.CoverFlow:
                case GUIFacadeControl.Layout.Filmstrip:
                case GUIFacadeControl.Layout.LargeIcons:
                case GUIFacadeControl.Layout.List:
                case GUIFacadeControl.Layout.SmallIcons:
                    return true;

                default:
                    return false;
            }
        }

        protected override void OnInfo(int iItem) {
            base.OnInfo(iItem);
        }

        public override void OnAction(MediaPortal.GUI.Library.Action action) {
            switch (action.wID) {
                case MediaPortal.GUI.Library.Action.ActionType.ACTION_PREVIOUS_MENU:
                    Navigation.FetchPreviousMenu(Navigation.CurrentItem);
                    break;
                default:
                    base.OnAction(action);
                    break;
            }
        }

        protected override void OnClicked(int controlId, GUIControl control, MediaPortal.GUI.Library.Action.ActionType actionType) {
            switch (controlId) {
                
                default:
                    break;
            }
            base.OnClicked(controlId, control, actionType);
        }

        protected override void OnClick(int iItem) {
            if (facadeLayout[iItem] is IMenuItem) {
                ((IMenuItem)facadeLayout[iItem]).OnClicked(this, null);
            } else {
                base.OnClick(iItem);
            }
        }

        protected override void OnShowContextMenu() {
            switch (Dialogs.ShowContextMenu()) {
                case Buttons.BtnSwitchLayout:
                    
                case Buttons.BtnSortAsc:
                    break;
                case Buttons.BtnSortDesc:
                    break;
                case Buttons.BtnSearch:
                    Dialogs.ShowSearchMenu();
                    break;
                case Buttons.NothingSelected:
                    break;
                default:
                    break;
            }
               
        }

        #endregion

        #region Private Methods

        private void RegisterEventHandlers() {
            PlexInterface.OnPlexError += new PlexInterface.OnPlexErrorEventHandler(PlexInterface_OnPlexError);
            MediaRetrieval.OnArtWorkRetrieved += new MediaRetrieval.OnArtWorkRetrievedEventHandler(MediaRetrieval_OnArtWorkRetrieved);
            PlexItem.OnHasBackground += new PlexItem.OnHasBackgroundEventHandler(MenuItem_OnHasBackground);
            MenuItem.OnMenuItemSelected += new MenuItem.OnMenuItemSelectedEventHandler(MenuItem_OnMenuItemSelected);
            PlexItem.OnPreferredLayout += new PlexItem.OnPreferredLayoutEventHandler(MenuItem_OnPreferredLayout);
            PlexItem.OnItemDetailsUpdated += new PlexItem.OnItemDetailsUpdatedEventHandler(PlexItem_OnItemDetailsUpdated);
            Navigation.OnMenuItemsFetched += new Navigation.OnMenuItemsFetchedEventHandler(Navigation_OnMenuItemsFetched);
        }
      
        private void UnRegisterEventHandlers() {
            PlexInterface.OnPlexError -= PlexInterface_OnPlexError;
            MediaRetrieval.OnArtWorkRetrieved -= MediaRetrieval_OnArtWorkRetrieved;
            PlexItem.OnHasBackground -= MenuItem_OnHasBackground;
            MenuItem.OnMenuItemSelected -= MenuItem_OnMenuItemSelected;
            PlexItem.OnPreferredLayout -= MenuItem_OnPreferredLayout;
            PlexItem.OnItemDetailsUpdated -= PlexItem_OnItemDetailsUpdated;
            Navigation.OnMenuItemsFetched -= Navigation_OnMenuItemsFetched;
        }

        #endregion

        #region Plugin Event Handlers

        void PlexInterface_OnPlexError(Exception plexError) {
            Dialogs.ShowNotifyDialog(30, plexError.ToString());
        }


        #endregion
    }
}
