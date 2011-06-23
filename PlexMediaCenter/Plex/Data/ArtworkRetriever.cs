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

        //public event OnArtworkRetrievalStartedEventHandler OnArtworkRetrievalStarted;
        //public delegate void OnArtworkRetrievalStartedEventHandler(string currentArtworl);

        //public event OnArtWorkRetrievedEventHandler OnArtWorkRetrieved;
        //public delegate void OnArtWorkRetrievedEventHandler(string artWork);

        //private Dictionary<string, string> ImageCache { get; set; }


        public string ImageBasePath { get; private set; }
        public string DefaultImagePath { get; private set; }

        public ArtworkRetriever(string basePath, string defaultImagePath) {
            //ImageCache = new Dictionary<string, string>();
            ImageBasePath = basePath;
            DefaultImagePath = defaultImagePath;
        }

        //private object sync = new object();

        //public string GetArtwork(string imageOnlinePath) {
        //    if (String.IsNullOrEmpty(imageOnlinePath)) {
        //        return DefaultImagePath;
        //    }
        //    if (File.Exists(imageOnlinePath)) {
        //        return imageOnlinePath;
        //    }
        //    string localImagePath = ImageBasePath;
        //    if (!Directory.Exists(localImagePath)) {
        //        Directory.CreateDirectory(localImagePath);
        //    }
        //    localImagePath = Path.Combine(localImagePath, GetSafeFilenameFromUrl(imageOnlinePath, '_'));
        //    if (File.Exists(localImagePath)) {
        //        return localImagePath;
        //    }
        //    lock (sync) {
        //        if (!ImageCache.ContainsKey(imageOnlinePath)) {
        //            ImageCache.Add(imageOnlinePath, localImagePath);
        //            try {
        //                DownloadImage(imageOnlinePath, localImagePath);
        //            } catch {
        //                //ToDo
        //            }
        //        }
        //        return ImageCache[imageOnlinePath];

        //    }
        //}

        //public void DownloadImage(string imageOnlinePath, string imageLocalPath) {
        //    WebClient ArtworkRetriever = new WebClient();
        //    PlexInterface.PlexServerCurrent.AddAuthHeaders(ref ArtworkRetriever);
        //    ArtworkRetriever.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(ArtWorkRetriever_DownloadFileCompleted);
        //    ArtworkRetriever.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ArtWorkRetriever_DownloadProgressChanged);
        //    ArtworkRetriever.DownloadFileAsync(new Uri(PlexInterface.PlexServerCurrent.UriPlexBase, imageOnlinePath), imageLocalPath, imageOnlinePath);
        //}

        //void ArtWorkRetriever_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
        //    if (e.Error != null || e.Cancelled) {
        //        throw e.Error;
        //    } else {
        //        if (e.UserState is string) {
        //            string artWorkIndex = (string)e.UserState;
        //            if (ImageCache.ContainsKey(artWorkIndex)) {
        //                OnArtWorkRetrieved(ImageCache[artWorkIndex]);
        //            }
        //        }
        //    }
        //}

        //void ArtWorkRetriever_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
        //    //throw new NotImplementedException();
        //}
            
        public void QueueArtwork( Action<string> downloadFinishedCallback, string imageUrl) {
            if (String.IsNullOrEmpty(imageUrl)) {
                return;
            }
            string localImagePath = ImageBasePath;
            if (!Directory.Exists(localImagePath)) {
                Directory.CreateDirectory(localImagePath);
            }
            localImagePath = Path.Combine(localImagePath, GetSafeFilenameFromUrl(imageUrl, '_'));
            if (File.Exists(localImagePath)) {
                //Image locally available so nothing to do...
                downloadFinishedCallback.Invoke(localImagePath);
            } else {
                //we need to put this in the Threadpool                
                ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadEnqueuedArtwork), new ArtworkQueueItem(imageUrl, localImagePath, downloadFinishedCallback));
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
            public string ImageUrl { get; set; }
            public string ImageLocalPath { get; set; }
            public Action<string> FinishedCallback { get; set; }

            public ArtworkQueueItem(string imageUrl, string imageLocalPath, Action<string> downloadFinishedCallback) { }
        }

        public static string GetSafeFilenameFromUrl(string url, char replaceChar) {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars()) {
                url = url.Replace(c, replaceChar);
            }
            return url;
        }
    }
}
