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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaPortal.GUI.Library;
using MyPlexMedia.Plugin.Config;
using MyPlexMedia.Plugin.Window.Dialogs;
using MyPlexMedia.Plugin.Window.Items;
using MyPlexMedia.Plugin.Window.Playback;
using PlexMediaCenter.Plex;
using WindowPlugins;
using System;
using System.Xml;

namespace MyPlexMedia.Plugin.Window {
    public partial class Main : WindowPluginBase {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Main() {
            GetID = Settings.PLUGIN_WINDOW_ID;
            logger.Debug("Main()");
        }

        #region Enums

        #endregion

        #region Skin Controls

        [SkinControlAttribute(2011)]
        protected GUIImage ctrlBackgroundImage;

        #endregion

        #region GUIWindow Base Class Overrides

        public override bool SupportsDelayedLoad {
            get { return true; }
        }

        protected override string SerializeName {
            get { return Settings.PLUGIN_NAME; }
        }

        public override bool Init() {
            Settings.Load();
            logger.Debug("Init()");

            return Load(Settings.SKINFILE_MAIN_WINDOW);
        }

        public override void DeInit() {
            base.DeInit();
            PlexInterface.DeInit();
            logger.Debug("DeInit()");
        }

        protected override void OnPageLoad() {
            GUIPropertyManager.SetProperty("#currentmodule", Settings.PLUGIN_NAME);
            GUIPropertyManager.SetProperty("#MyPlexMedia.Buffering.State", string.Empty);
            if (!PlexInterface.Initialized) {
                PlexInterface.Init(Settings.PLEX_SERVER_LIST_XML, Settings.CacheFolder, Settings.DownloadArtwork);
                if (!String.IsNullOrEmpty(Settings.MyPlexUser) && !String.IsNullOrEmpty(Settings.MyPlexPass)) {
                    PlexInterface.MyPlexLogin(Settings.MyPlexUser, Settings.MyPlexPass);
                }
                //FacadeVideo = facadeLayout;
                //TryLoadFacades();
            }
            RegisterEventHandlers();
            SetBackgroundImage(Settings.PLEX_BACKGROUND_DEFAULT);
            Navigation.ShowCurrentMenu(Navigation.CurrentItem, 0);
            logger.Info("Init()");
        }

        protected override void OnPageDestroy(int new_windowId) {
            Settings.Save();
            UnRegisterEventHandlers();
            base.OnPageDestroy(new_windowId);
            logger.Info("OnPageDestroy({0})", new_windowId);
        }

        protected override void SwitchLayout() {
            Navigation.CurrentItem.PreferredLayout = new Settings.PlexSectionLayout {
                Layout = CurrentLayout,
                Section = Navigation.CurrentItem.PreferredLayout.Section
            };
            //if (TryLoadFacade(Path.Combine(GUIGraphicsContext.Skin,"common.facade.music.xml"), out FacadeAudio)) {

            //}
            //if (TryLoadFacade(Path.Combine(GUIGraphicsContext.Skin, "common.facade.video.Title.xml"), out FacadeVideo)) {

            //}
            //if (TryLoadFacade(Path.Combine(GUIGraphicsContext.Skin, "common.facade.pictures.xml"), out FacadePictures)) {

            //}

            switch (Navigation.CurrentItem.PreferredLayout.Section) {
                case Settings.SectionType.Music:
                    LoadFacade(Path.Combine(GUIGraphicsContext.Skin, "common.facade.music.xml"));
                    break;
                case Settings.SectionType.Photo:
                    LoadFacade(Path.Combine(GUIGraphicsContext.Skin, "common.facade.pictures.xml"));
                    break;
                default:
                case Settings.SectionType.Video:
                    LoadFacade(Path.Combine(GUIGraphicsContext.Skin, "common.facade.video.Title.xml"));
                    break;
            }
            base.SwitchLayout();
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

        protected override void OnInfo(int item) {
            if (facadeLayout[item] is IMenuItem) {
                ((IMenuItem)facadeLayout[item]).Parent.LastSelectedChildIndex = item;
                ((IMenuItem)facadeLayout[item]).OnInfo();
            } else {
                base.OnInfo(item);
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

        protected override void OnClick(int iItem) {
            GUIListItem item = facadeLayout[iItem];
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

        private void LoadFacade(string xmlFile) {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFile);
            XmlNode node = doc.SelectSingleNode("//*[type='group']");
            try {
                lock (GUIGraphicsContext.RenderLock) {
                    facadeLayout.Clear();
                    GUIGroup group = (GUIGroup)GUIControlFactory.Create(GetID, node, new Dictionary<string, string>(), Path.Combine(GUIGraphicsContext.Skin, xmlFile));
                    group.WindowId = GetID;
                    group.PreAllocResources();
                    group.AllocResources();
                    facadeLayout = (GUIFacadeControl)group[0];
                    controlList[7] = group;
                }
            } catch (Exception ex) {
                Log.Error("Unable to load control. exception:{0}", ex.ToString());
            }
        }

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
            //ctrlBackgroundImage.BringIntoView();
            //ctrlBackgroundImage.DoUpdate();
            //ctrlBackgroundImage.Refresh();
            //GUIWindowManager.Process();
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

        private void RegisterEventHandlers() {
            PlexInterface.OnPlexError += PlexInterface_OnPlexError;
            PlexInterface.OnResponseProgress += PlexInterface_OnResponseProgress;
            MenuItem.OnMenuItemSelected += MenuItem_OnMenuItemSelected;
            Navigation.OnMenuItemsFetchStarted += Navigation_OnMenuItemsFetchStarted;
            Navigation.OnMenuItemsFetchCompleted += Navigation_OnMenuItemsFetchCompleted;
            Navigation.OnErrorOccured += PlexInterface_OnPlexError;
        }

        private void UnRegisterEventHandlers() {
            PlexInterface.OnPlexError -= PlexInterface_OnPlexError;
            PlexInterface.OnResponseProgress -= PlexInterface_OnResponseProgress;
            MenuItem.OnMenuItemSelected -= MenuItem_OnMenuItemSelected;
            Navigation.OnMenuItemsFetchStarted -= Navigation_OnMenuItemsFetchStarted;
            Navigation.OnMenuItemsFetchCompleted -= Navigation_OnMenuItemsFetchCompleted;
            Navigation.OnErrorOccured -= PlexInterface_OnPlexError;
        }

        #endregion

        #region Plugin Event Handlers

        #endregion
    }
}