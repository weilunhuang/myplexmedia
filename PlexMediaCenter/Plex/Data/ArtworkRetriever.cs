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
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using PlexMediaCenter.Util;

namespace PlexMediaCenter.Plex.Data {

    public class ArtworkRetriever : BackgroundWorker, IDisposable {

        #region Nested type: ArtworkQueueItem

        private class ArtworkQueueItem {
            public ArtworkQueueItem(Uri imageServerPath, string imageLocalPath, string imageFallback,
                                    Action<string> downloadFinishedCallback) {
                ImageUrl = imageServerPath;
                ImageLocalPath = imageLocalPath;
                ImageFallback = imageFallback;
                FinishedCallback = downloadFinishedCallback;
            }

            public Uri ImageUrl { get; private set; }
            public string ImageLocalPath { get; private set; }
            public string ImageFallback { get; private set; }
            public Action<string> FinishedCallback { get; private set; }
        }

        #endregion


        private const int MAX_CLIENTS = 5;
        private BlockingQueue<WebClient> QueueClients { get; set; }
        private BlockingQueue<ArtworkQueueItem> QueueArtwork { get; set; }
        private bool EnableDownload { get; set; }
        public string ImageBasePath { get; private set; }

        #region Delegates

        public delegate void OnArtworkRetrievalErrorEventHandler(PlexException e);
        public event OnArtworkRetrievalErrorEventHandler OnArtworkRetrievalError = delegate { };
        #endregion

        public ArtworkRetriever(string basePath, bool enableDownload) {
            WorkerSupportsCancellation = false;
            WorkerReportsProgress = false;
            if (!Directory.Exists(basePath)) {
                Directory.CreateDirectory(basePath);
            }
            ImageBasePath = basePath;
            EnableDownload = enableDownload;
            QueueClients = new BlockingQueue<WebClient>(MAX_CLIENTS);
            QueueArtwork = new BlockingQueue<ArtworkQueueItem>(int.MaxValue);

            if (enableDownload) {
                for (int i = 0; i < MAX_CLIENTS; ++i) {
                    var cli = new WebClient();
                    cli.DownloadFileCompleted += DownloadFileCompleted;
                    QueueClients.Enqueue(cli);
                }

                RunWorkerAsync();
            }
        }

        protected override void OnDoWork(DoWorkEventArgs e) {
            foreach (var client in QueueClients) {
                ArtworkQueueItem currentItem = QueueArtwork.Dequeue();
                if (currentItem == null) {
                    QueueClients.Enqueue(client);
                    continue;
                }
                try {
                    PlexInterface.GetPlexServerFromUri(currentItem.ImageUrl).CurrentConnection.AddAuthHeaders(client);
                    if (!File.Exists(currentItem.ImageLocalPath)) {
                        client.DownloadFileAsync(currentItem.ImageUrl, currentItem.ImageLocalPath, currentItem);
                    }
                    currentItem.FinishedCallback.Invoke(currentItem.ImageLocalPath);
                } catch (Exception ee) {
                    OnArtworkRetrievalError(new PlexException(GetType(), "Download failed: " + currentItem.ImageUrl, ee));
                    currentItem.FinishedCallback.Invoke(currentItem.ImageFallback);
                }
            }
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e) {
            base.OnRunWorkerCompleted(e);
        }

        void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
            if (e.UserState != null && e.UserState is ArtworkQueueItem) {
                ArtworkQueueItem currentItem = e.UserState as ArtworkQueueItem;
                currentItem.FinishedCallback.Invoke(currentItem.ImageLocalPath);
            }
            QueueClients.Enqueue((WebClient)sender);
        }

        public void ResetQueue() {
            QueueArtwork.Reset();
        }

        public void QueueArtworkItem(Action<string> downloadFinishedCallback, string fallbackImage, Uri sourcePath, string imageRelativePath) {
            if (imageRelativePath == null || string.IsNullOrEmpty(imageRelativePath) || !EnableDownload) {
                downloadFinishedCallback.Invoke(fallbackImage);
                return;
            }
            string localImagePath = Path.Combine(ImageBasePath, GetSafeFilenameFromUrl(imageRelativePath, '_'));
            if (File.Exists(localImagePath)) {
                //Image locally available so nothing to do...
                downloadFinishedCallback.Invoke(localImagePath);
            } else {
                QueueArtwork.Enqueue(new ArtworkQueueItem(new Uri(new UriBuilder(sourcePath.Scheme, sourcePath.Host, sourcePath.Port).Uri, imageRelativePath), localImagePath, fallbackImage,
                                                                   downloadFinishedCallback));
            }
        }

        public static string GetSafeFilenameFromUrl(string url, char replaceChar) {
            return System.IO.Path.GetInvalidFileNameChars().Aggregate(url,
                                                                      (current, c) => current.Replace(c, replaceChar));
        }

        public void Dispose() {
            try {
                QueueClients.Dispose();
                QueueArtwork.Dispose();
            } catch { }
        }
    }
}