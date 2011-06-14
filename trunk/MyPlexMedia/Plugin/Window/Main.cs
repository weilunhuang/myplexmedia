using System;
using System.Collections.Generic;
using System.Threading;
using MediaPortal.GUI.Library;
using MyPlexMedia.Plugin.Config;
using MyPlexMedia.Plugin.Window.Items;
using WindowPlugins;
using PlexMediaCenter.Plex;
using PlexMediaCenter.Util;

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
            PlexInterface.OnPlexError += new PlexInterface.OnPlexErrorEventHandler(PlexInterface_OnPlexError);
            MediaRetrieval.OnArtWorkRetrieved += new MediaRetrieval.OnArtWorkRetrievedEventHandler(MediaRetrieval_OnArtWorkRetrieved);
            PlexInterface.Init(Settings.PLEX_SERVER_LIST_XML, Settings.PLEX_ARTWORK_ROOT_PATH);
            return Load(GUIGraphicsContext.Skin + @"\MyPlexMedia.xml");
        }

        void MediaRetrieval_OnArtWorkRetrieved(string artWork) {
            if (facadeLayout.NeedRefresh()) {
                facadeLayout.DoUpdate();
            }
        }

        public override void DeInit() {
            base.DeInit();
        }

        protected override void LoadSettings() {
            Settings.Load();
        }

        protected override void OnPageLoad() {
            base.OnPageLoad();
            Navigation.OnMenuItemsFetched += new Navigation.OnMenuItemsFetchedEventHandler(Navigation_OnMenuItemsFetched);
            Navigation.CreateStartupMenu();
            CurrentLayout = GUIFacadeControl.Layout.CoverFlow;
            SwitchLayout();
        }

        protected override void OnPageDestroy(int new_windowId) {
            Navigation.OnMenuItemsFetched -= Navigation_OnMenuItemsFetched;
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
                case (int)Buttons.BtnBrowseLatestCheez:
                    GUIPropertyManager.SetProperty("#currentmodule", Settings.PLUGIN_NAME + " (online mode)");
                    break;
                case (int)Buttons.BtnBrowseLocalCheez:
                    GUIPropertyManager.SetProperty("#currentmodule", Settings.PLUGIN_NAME + " (local mode)");
                    break;
                default:
                    break;
            }
            base.OnClicked(controlId, control, actionType);
        }

        protected override void OnClick(int iItem) {
            if (facadeLayout[iItem] is IMenuItem) {
                ctrlBackgroundImage.SetFileName(((IMenuItem)facadeLayout[iItem]).BackgroundImage);
                ctrlBackgroundImage.DoUpdate();
                ctrlBackgroundImage.Refresh();


                ((IMenuItem)facadeLayout[iItem]).OnClicked(this, null);
            } else {
                base.OnClick(iItem);
            }
        }

        protected override void OnShowContextMenu() {
            //    switch (Dialogs.ShowContextMenu()) {
            //        case Buttons.BtnCheezSitesOverview:
            //            DisplayCheezSitesOverview();
            //            break;
            //        case Buttons.BtnSwitchLayout:
            //            OnShowLayouts();
            //            break;
            //        case Buttons.BtnBrowseLatestCheez:

            //            break;
            //        case Buttons.BtnBrowseLocalCheez:

            //            break;
            //        case Buttons.BtnBrowseRandomCheez:

            //            break;
            //        case Buttons.BtnBrowseMore:

            //            break;
            //        case Buttons.BtnSortAsc:
            //            if (facadeLayout != null) {
            //                facadeLayout.Sort(new CheezComparerDateAsc());
            //                this.Process();
            //            }
            //            break;
            //        case Buttons.BtnSortDesc:
            //            if (facadeLayout != null) {
            //                facadeLayout.Sort(new CheezComparerDateDesc());
            //                this.Process();
            //            }
            //            break;
            //        case Buttons.BtnShowSlideShowAllLocal:
            //            OnSlideShowAllLocal();
            //            break;
            //        case Buttons.BtnShowSlideShowCurrent:

            //            OnSlideShowCurrent();
            //            break;
            //        case Buttons.BtnCancelAllDownloads:

            //            break;
            //        case Buttons.BtnDeleteLocalCheez:

            //            break;
            //        case Buttons.NothingSelected:
            //        default:
            //            //throw new ArgumentOutOfRangeException();
            //            return;
            //    }
        }

        #endregion

        #region Private Methods

        #endregion

        #region Plugin Event Handlers

        void PlexInterface_OnPlexError(Exception plexError) {
            Dialogs.ShowNotifyDialog(30, plexError.ToString());
        }

        void Navigation_OnMenuItemsFetched(List<IMenuItem> fetchedMenuItems) {
            facadeLayout.Clear();
            foreach (var item in fetchedMenuItems) {
                if (item is MenuItem) {
                    facadeLayout.Add((MenuItem)item);
                }
            }
            facadeLayout.DoUpdate();
        }

        #endregion
    }
}
