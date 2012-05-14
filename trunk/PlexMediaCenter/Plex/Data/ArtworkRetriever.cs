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
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using PlexMediaCenter.Plex.Connection;

namespace PlexMediaCenter.Plex.Data {
    public class ArtworkRetriever {
        #region Delegates

        public delegate void OnArtworkRetrievalErrorEventHandler(PlexException e);
        public event OnArtworkRetrievalErrorEventHandler OnArtworkRetrievalError = delegate { };
        #endregion

        public ArtworkRetriever(string basePath, string defaultImagePath) {
            ImageBasePath = basePath;
            DefaultImagePath = defaultImagePath;
        }

        public string ImageBasePath { get; private set; }
        public string DefaultImagePath { get; private set; }
       

        public void QueueArtwork(Action<string> downloadFinishedCallback, Uri sourcePath, string imageRelativePath) {
            if (imageRelativePath == null || string.IsNullOrEmpty(imageRelativePath)) {
                return;
            }
            string localImagePath = ImageBasePath;
            if (!Directory.Exists(localImagePath)) {
                Directory.CreateDirectory(localImagePath);
            }
            localImagePath = Path.Combine(localImagePath, GetSafeFilenameFromUrl(imageRelativePath, '_'));
            if (File.Exists(localImagePath)) {
                //Image locally available so nothing to do...
                downloadFinishedCallback.Invoke(localImagePath);
            } else {
                //we need to put this in the Threadpool                 
                int max;
                int maxcomp;
                ThreadPool.GetMaxThreads(out max, out maxcomp);
                ThreadPool.SetMaxThreads(max, 10);
                ThreadPool.QueueUserWorkItem(DownloadEnqueuedArtwork,
                                             new ArtworkQueueItem(new Uri(new UriBuilder(sourcePath.Scheme, sourcePath.Host, sourcePath.Port).Uri, imageRelativePath), localImagePath,
                                                                  downloadFinishedCallback));
            }
        }

        private void DownloadEnqueuedArtwork(object queueItem) {
            ArtworkQueueItem currentItem = queueItem as ArtworkQueueItem;
            WebClient downloader = new WebClient();
            try {
                PlexInterface.GetPlexServerFromUri(currentItem.ImageUrl).CurrentConnection.AddAuthHeaders(ref downloader);
                if (!File.Exists(currentItem.ImageLocalPath)) {
                    downloader.DownloadFile(currentItem.ImageUrl, currentItem.ImageLocalPath);
                }
                currentItem.FinishedCallback.Invoke(currentItem.ImageLocalPath);
            } catch (Exception e) {
                OnArtworkRetrievalError(new PlexException(GetType(), "Download failed: " + currentItem.ImageUrl, e));
            }
            downloader.Dispose();
        }

        public static string GetSafeFilenameFromUrl(string url, char replaceChar) {
            return System.IO.Path.GetInvalidFileNameChars().Aggregate(url,
                                                                      (current, c) => current.Replace(c, replaceChar));
        }

        #region Nested type: ArtworkQueueItem

        private class ArtworkQueueItem {
            public ArtworkQueueItem(Uri imageServerPath, string imageLocalPath,
                                    Action<string> downloadFinishedCallback) {
                ImageUrl = imageServerPath;
                ImageLocalPath = imageLocalPath;
                FinishedCallback = downloadFinishedCallback;
            }

            public Uri ImageUrl { get; private set; }
            public string ImageLocalPath { get; private set; }
            public Action<string> FinishedCallback { get; private set; }
        }

        #endregion
    }
}