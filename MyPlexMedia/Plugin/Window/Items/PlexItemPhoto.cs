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
using System.Linq;
using MediaPortal.GUI.Library;
using MediaPortal.GUI.Pictures;
using MediaPortal.GUI.Video;
using MediaPortal.Video.Database;
using MyPlexMedia.Plugin.Config;
using MyPlexMedia.Plugin.Window.Playback;
using PlexMediaCenter.Plex;
using PlexMediaCenter.Plex.Data.Types;

namespace MyPlexMedia.Plugin.Window.Items {
    public class PlexItemPhoto : PlexItemBase {

        public MediaContainerPhoto Photo { get; set; }
        private IMDBMovie MovieDetails { get; set; }

        public PlexItemPhoto(IMenuItem parentItem, string title, Uri path, MediaContainerPhoto photo)
            : base(parentItem, title, path) {
            Photo = photo;
            RetrieveArt = true;
            OnRetrieveArt += new RetrieveCoverArtHandler(PlexItemVideo_OnRetrieveArt);
        }

        void PlexItemVideo_OnRetrieveArt(GUIListItem item) {
            PlexInterface.ArtworkRetriever.QueueArtworkItem(SetIcon, Settings.PLEX_ICON_DEFAULT, UriPath, Photo.thumb);
            //PlexInterface.ArtworkRetriever.QueueArtworkItem(SetBackground, Settings.PLEX_BACKGROUND_DEFAULT, UriPath, Photo.art);
        }

        public override void SetMetaData(MediaContainer infoContainer) {

        }

        public override void OnSelected() {
        }

        public override void OnClicked(object sender, EventArgs e) {
            PlexInterface.ArtworkRetriever.QueueArtworkItem(ImageLoaded, Settings.PLEX_BACKGROUND_DEFAULT, UriPath, Photo.Media[0].Part[0].key);
        }

        private void ImageLoaded(string imagePath) {
            GUISlideShow SlideShow = (GUISlideShow)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_SLIDESHOW);
            SlideShow.Reset();
            SlideShow.Add(imagePath);
            SlideShow.Select(imagePath);
            SlideShow.StartSlideShow();
            GUIWindowManager.ActivateWindow((int)GUIWindow.Window.WINDOW_SLIDESHOW);
        }
    }
}