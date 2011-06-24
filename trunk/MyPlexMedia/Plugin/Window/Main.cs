using System.Collections.Generic;
using MediaPortal.GUI.Library;
using MyPlexMedia.Plugin.Config;
using MyPlexMedia.Plugin.Window.Items;
using PlexMediaCenter.Plex;
using PlexMediaCenter.Plex.Data;
using WindowPlugins;

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
            facadeLayout.Clear();
            RegisterEventHandlers();
            if (Navigation.CurrentItem == null) {
                facadeLayout.Clear();
                Navigation.CreateStartupMenu(Settings.LastPlexServer);
                CurrentLayout = Settings.DefaultLayout;
                SwitchLayout();
            } else {
                Navigation.ShowCurrentMenu(Navigation.CurrentItem, 0);
            }
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
            if (facadeLayout[iItem] is IMenuItem) {
                ((IMenuItem)facadeLayout[iItem]).Parent.LastSelectedChildIndex = iItem;
                ((IMenuItem)facadeLayout[iItem]).OnInfo();
            } else {
                base.OnInfo(iItem);
            }            
        }

        public override void OnAction(MediaPortal.GUI.Library.Action action) {
            switch (action.wID) {
                case MediaPortal.GUI.Library.Action.ActionType.ACTION_PREVIOUS_MENU:
                    if (Navigation.CurrentItem == Navigation.RootItem) {
                        base.OnAction(action);
                        return;
                    }
                    Navigation.FetchPreviousMenu(Navigation.CurrentItem, facadeLayout.SelectedListItemIndex);
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
                ((IMenuItem)facadeLayout[iItem]).Parent.LastSelectedChildIndex = iItem;
                ((IMenuItem)facadeLayout[iItem]).OnClicked(this, null);                
            } else {
                base.OnClick(iItem);
            }            
        }

        protected override void OnShowContextMenu() {
            Navigation.ShowCurrentContextMenu();
        }

        #endregion

        #region Private Methods

        private void RegisterEventHandlers() {
            PlexInterface.OnPlexError += new PlexInterface.OnPlexErrorEventHandler(PlexInterface_OnPlexError);
            PlexInterface.OnResponseProgress += new PlexInterface.OnResponseProgressEventHandler(PlexInterface_OnResponseProgress);
            PlexItemBase.OnHasBackground += new PlexItemBase.OnHasBackgroundEventHandler(MenuItem_OnHasBackground);
            MenuItem.OnMenuItemSelected += new MenuItem.OnMenuItemSelectedEventHandler(MenuItem_OnMenuItemSelected);
            Navigation.OnMenuItemsFetchStarted += new Navigation.OnMenuItemsFetchStartedEventHandler(Navigation_OnMenuItemsFetchStarted);
            Navigation.OnMenuItemsFetchCompleted += new Navigation.OnMenuItemsFetchCompletedEventHandler(Navigation_OnMenuItemsFetchCompleted);
            Navigation.OnErrorOccured += new Navigation.OnErrorOccuredEventHandler(PlexInterface_OnPlexError);
        }

        private void UnRegisterEventHandlers() {
            PlexInterface.OnPlexError -= PlexInterface_OnPlexError;
            MenuItem.OnMenuItemSelected -= MenuItem_OnMenuItemSelected;
            Navigation.OnMenuItemsFetchCompleted -= Navigation_OnMenuItemsFetchCompleted;
            Navigation.OnMenuItemsFetchStarted -= Navigation_OnMenuItemsFetchStarted;
            Navigation.OnErrorOccured -= PlexInterface_OnPlexError;
        }

        #endregion

        #region Plugin Event Handlers


        #endregion
    }
}
