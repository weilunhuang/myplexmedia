using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using PlexMediaCenter.Plex.Connection;


namespace PlexMediaCenter.Plex.Data {
    public class ArtworkRetriever {

        public event OnArtworkRetrievalErrorEventHandler OnArtworkRetrievalError;
        public delegate void OnArtworkRetrievalErrorEventHandler(PlexException e);

        public string ImageBasePath { get; private set; }
        public string DefaultImagePath { get; private set; }

        public ArtworkRetriever(string basePath, string defaultImagePath) {
            ImageBasePath = basePath;
            DefaultImagePath = defaultImagePath;
        }

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
                ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadEnqueuedArtwork), new ArtworkQueueItem(plexServer, imageFileName, localImagePath, downloadFinishedCallback));
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
                OnArtworkRetrievalError(new PlexException(this.GetType(), "Download failed: " + currentItem.ImageUrl, e));
            }            
            downloader.Dispose();
        }

        class ArtworkQueueItem {
            public PlexServer PlexServer { get; set; }
            public Uri ImageUrl { get; set; }
            public string ImageLocalPath { get; set; }
            public Action<string> FinishedCallback { get; set; }

            public ArtworkQueueItem(PlexServer plexServer, string imageServerPath, string imageLocalPath, Action<string> downloadFinishedCallback) {
                PlexServer = plexServer;
                ImageUrl = new Uri(plexServer.UriPlexBase, imageServerPath);
                ImageLocalPath = imageLocalPath;
                FinishedCallback = downloadFinishedCallback;
            }
        }

        public static string GetSafeFilenameFromUrl(string url, char replaceChar) {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars()) {
                url = url.Replace(c, replaceChar);
            }
            return url;
        }
    }
}
