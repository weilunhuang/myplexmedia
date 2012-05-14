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

namespace PlexMediaCenter.Plex.Data {
    public class ArtworkRetriever : BackgroundWorker, IDisposable {

        private const int MAX_CLIENTS = 5;
        private BlockingQueue<WebClient> QueueClients;
        private BlockingQueue<ArtworkQueueItem> QueueArtwork;
        public string ImageBasePath { get; private set; }

        #region Delegates

        public delegate void OnArtworkRetrievalErrorEventHandler(PlexException e);
        public event OnArtworkRetrievalErrorEventHandler OnArtworkRetrievalError = delegate { };
        #endregion

        public ArtworkRetriever(string basePath) {
            WorkerSupportsCancellation = false;
            WorkerReportsProgress = false;
            if (!Directory.Exists(basePath)) {
                Directory.CreateDirectory(basePath);
            }
            ImageBasePath = basePath;
            QueueClients = new BlockingQueue<WebClient>(MAX_CLIENTS);
            for (int i = 0; i < MAX_CLIENTS; ++i) {
                var cli = new WebClient();
                cli.DownloadFileCompleted += DownloadFileCompleted;
                QueueClients.Enqueue(cli);
            }

            QueueArtwork = new BlockingQueue<ArtworkQueueItem>(int.MaxValue);
            RunWorkerAsync();
        }

        protected override void OnDoWork(DoWorkEventArgs e) {
            foreach (var client in QueueClients) {
                ArtworkQueueItem currentItem = QueueArtwork.Dequeue();
                try {
                    PlexInterface.GetPlexServerFromUri(currentItem.ImageUrl).CurrentConnection.AddAuthHeaders(client);
                    if (!File.Exists(currentItem.ImageLocalPath)) {
                        client.DownloadFileAsync(currentItem.ImageUrl, currentItem.ImageLocalPath, currentItem);
                    }
                    currentItem.FinishedCallback.Invoke(currentItem.ImageLocalPath);
                } catch (Exception ee) {
                    OnArtworkRetrievalError(new PlexException(GetType(), "Download failed: " + currentItem.ImageUrl, ee));
                }
            }
        }

        void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
            if (e.UserState != null && e.UserState is ArtworkQueueItem) {
                ArtworkQueueItem currentItem = e.UserState as ArtworkQueueItem;
                currentItem.FinishedCallback.Invoke(currentItem.ImageLocalPath);
            }
        }

        public void ResetQueue() {
            QueueArtwork.Reset();
        }

        public void QueueArtworkItem(Action<string> downloadFinishedCallback, Uri sourcePath, string imageRelativePath) {
            if (imageRelativePath == null || string.IsNullOrEmpty(imageRelativePath)) {
                return;
            }
            string localImagePath = Path.Combine(ImageBasePath, GetSafeFilenameFromUrl(imageRelativePath, '_'));
            if (File.Exists(localImagePath)) {
                //Image locally available so nothing to do...
                downloadFinishedCallback.Invoke(localImagePath);
            } else {
               QueueArtwork.Enqueue(new ArtworkQueueItem(new Uri(new UriBuilder(sourcePath.Scheme, sourcePath.Host, sourcePath.Port).Uri, imageRelativePath), localImagePath,
                                                                  downloadFinishedCallback));
            }
        }

        //private void DownloadEnqueuedArtwork(object queueItem) {
        //    ArtworkQueueItem currentItem = queueItem as ArtworkQueueItem;
        //    WebClient downloader = new WebClient();
        //    try {
        //        PlexInterface.GetPlexServerFromUri(currentItem.ImageUrl).CurrentConnection.AddAuthHeaders(downloader);
        //        if (!File.Exists(currentItem.ImageLocalPath)) {
        //            downloader.DownloadFile(currentItem.ImageUrl, currentItem.ImageLocalPath);
        //        }
        //        currentItem.FinishedCallback.Invoke(currentItem.ImageLocalPath);
        //    } catch (Exception e) {
        //        OnArtworkRetrievalError(new PlexException(GetType(), "Download failed: " + currentItem.ImageUrl, e));
        //    }
        //    downloader.Dispose();
        //}

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

        #region Nested type: BlockingQueue

        private class BlockingQueue<T> : IDisposable {
            private Queue<T> _queue;
            private Semaphore _semaphore;

            public BlockingQueue(int maxThreads) {
                _queue = new Queue<T>();
                _semaphore = new Semaphore(0, maxThreads);
            }

            public void Reset() {
                lock (_queue) _queue.Clear();
            }

            public void Enqueue(T data) {
                if (data == null) throw new ArgumentNullException("data");
                lock (_queue) _queue.Enqueue(data);
                _semaphore.Release();
            }

            public T Dequeue() {
                _semaphore.WaitOne();
                lock (_queue) return _queue.Dequeue();
            }

            public IEnumerator<T> GetEnumerator() {
                while (true) yield return Dequeue();
            }

            public void Dispose() {
                if (_semaphore != null) {
                    _semaphore.Close();
                    _semaphore = null;
                }
            }
        }
        #endregion

        public void Dispose() {
            QueueClients.Dispose();
            QueueArtwork.Dispose();
        }
    }

}