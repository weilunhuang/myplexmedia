using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;


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
            
        public void QueueArtwork( Action<string> downloadFinishedCallback, Uri serverBasePath, string imageFileName) {
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
                ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadEnqueuedArtwork), new ArtworkQueueItem(new Uri(serverBasePath, imageFileName), localImagePath, downloadFinishedCallback));               
            }
        }

        private void DownloadEnqueuedArtwork(object queueItem) {
            ArtworkQueueItem currentItem = queueItem as ArtworkQueueItem;
            using (WebClient downloader = new WebClient()) {
                try {
                    downloader.DownloadFile(currentItem.ImageUrl, currentItem.ImageLocalPath);
                    currentItem.FinishedCallback.Invoke(currentItem.ImageLocalPath);
                } catch (Exception e) {
                    OnArtworkRetrievalError(new PlexException(this.GetType(), "Download failed: " + currentItem.ImageUrl, e));
                }
            }            
        }

        class ArtworkQueueItem {
            public Uri ImageUrl { get; set; }
            public string ImageLocalPath { get; set; }
            public Action<string> FinishedCallback { get; set; }

            public ArtworkQueueItem(Uri imageUrl, string imageLocalPath, Action<string> downloadFinishedCallback) {
                ImageUrl = imageUrl;
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
