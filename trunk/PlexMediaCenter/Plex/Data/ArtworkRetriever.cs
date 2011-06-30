﻿#region #region Copyright (C) 2005-2011 Team MediaPortal

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
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using PlexMediaCenter.Plex.Connection;

namespace PlexMediaCenter.Plex.Data {
    public class ArtworkRetriever {
        #region Delegates

        public delegate void OnArtworkRetrievalErrorEventHandler(PlexException e);

        #endregion

        public ArtworkRetriever(string basePath, string defaultImagePath) {
            ImageBasePath = basePath;
            DefaultImagePath = defaultImagePath;
        }

        public string ImageBasePath { get; private set; }
        public string DefaultImagePath { get; private set; }
        public event OnArtworkRetrievalErrorEventHandler OnArtworkRetrievalError;

        public void QueueArtwork(Action<string> downloadFinishedCallback, PlexServer plexServer, string imageFileName) {
            if (String.IsNullOrEmpty(imageFileName)) {
                return;
            }
            string localImagePath = ImageBasePath;
            if (!Directory.Exists(localImagePath)) {
                Directory.CreateDirectory(localImagePath);
            }
            localImagePath = Path.Combine(localImagePath, GetSafeFilenameFromUrl(imageFileName, '_'));
            if (File.Exists(localImagePath)) {
                //Image locally available so nothing to do...
                downloadFinishedCallback.Invoke(localImagePath);
            } else {
                //we need to put this in the Threadpool                 
                ThreadPool.QueueUserWorkItem(DownloadEnqueuedArtwork,
                                             new ArtworkQueueItem(plexServer, imageFileName, localImagePath,
                                                                  downloadFinishedCallback));
            }
        }

        private void DownloadEnqueuedArtwork(object queueItem) {
            ArtworkQueueItem currentItem = queueItem as ArtworkQueueItem;
            WebClient downloader = new WebClient();
            try {
                currentItem.PlexServer.AddAuthHeaders(ref downloader);
                downloader.DownloadFile(currentItem.ImageUrl, currentItem.ImageLocalPath);
                currentItem.FinishedCallback.Invoke(currentItem.ImageLocalPath);
            } catch (Exception e) {
                OnArtworkRetrievalError(new PlexException(GetType(), "Download failed: " + currentItem.ImageUrl, e));
            }
            downloader.Dispose();
        }

        public static string GetSafeFilenameFromUrl(string url, char replaceChar) {
            return System.IO.Path.GetInvalidFileNameChars().Aggregate(url, (current, c) => current.Replace(c, replaceChar));
        }

        #region Nested type: ArtworkQueueItem

        private class ArtworkQueueItem {
            public ArtworkQueueItem(PlexServer plexServer, string imageServerPath, string imageLocalPath,
                                    Action<string> downloadFinishedCallback) {
                PlexServer = plexServer;
                ImageUrl = new Uri(plexServer.UriPlexBase, imageServerPath);
                ImageLocalPath = imageLocalPath;
                FinishedCallback = downloadFinishedCallback;
            }

            public PlexServer PlexServer { get; private set; }
            public Uri ImageUrl { get; private set; }
            public string ImageLocalPath { get; private set; }
            public Action<string> FinishedCallback { get; private set; }
        }

        #endregion
    }
}