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
using PlexMediaCenter.Plex;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Plex.Connection;
using MyPlexMedia.Plugin.Config;

namespace MyPlexMedia.Plugin.Window.Items {
    internal class PlexItemDirectory : PlexItemBase {
        public PlexItemDirectory(IMenuItem parentItem, string title, Uri path, MediaContainerDirectory directory)
            : base(parentItem, title, path) {
            Directory = directory;
            IsFolder = true;
            RetrieveArt = true;
            OnRetrieveArt += new RetrieveCoverArtHandler(PlexItemDirectory_OnRetrieveArt);

            int duration;
            if (int.TryParse(Directory.duration, out duration)) {
                Duration = duration;
            }
            if (!String.IsNullOrEmpty(Directory.viewedLeafCount) && !String.IsNullOrEmpty(Directory.leafCount)) {
                Label2 += String.Format(" [{0}/{1}]", Directory.viewedLeafCount, Directory.leafCount);
            }
            //Label3 = Directory.summary;
            FileInfo = new MediaPortal.Util.FileInformation();
            if (!String.IsNullOrEmpty(Directory.originallyAvailableAt)) {
                FileInfo.CreationTime = DateTime.Parse(Directory.originallyAvailableAt);
            }
        }

        void PlexItemDirectory_OnRetrieveArt(MediaPortal.GUI.Library.GUIListItem item) {
            if (item.Equals(this)) {
                if (Directory.thumb != null) {
                    PlexInterface.ArtworkRetriever.QueueArtworkItem(SetIcon, UriPath, Directory.thumb);
                } else {
                    SetIcon(Settings.PLEX_ICON_DEFAULT);
                }
                if (Directory.art != null) {
                    PlexInterface.ArtworkRetriever.QueueArtworkItem(SetBackground, UriPath, Directory.art);
                } else {
                    SetBackground(Settings.PLEX_BACKGROUND_DEFAULT);
                }
            }
        }

        public MediaContainerDirectory Directory { get; set; }

        public override void OnInfo() {
            if (String.IsNullOrEmpty(Directory.type)) return;
            switch (Directory.type) {
                case "artist":
                    break;
                case "album":
                    break;
                default:
                    break;
            }
        }
    }
}