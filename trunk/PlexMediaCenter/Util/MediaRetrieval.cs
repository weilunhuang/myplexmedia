using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Net;
using System.IO;
using PlexMediaCenter.Plex;

namespace PlexMediaCenter.Util {
    public static class MediaRetrieval {

        public static event OnArtWorkRetrievedEventHandler OnArtWorkRetrieved;
        public delegate void OnArtWorkRetrievedEventHandler(string artWork);

        private static Dictionary<string, string> ImageCache { get; set; }

        public static string DefaulIconPath { get; set; }

        static MediaRetrieval() {
            ImageCache = new Dictionary<string, string>();
            foreach (string plexType in Enum.GetNames(typeof(EPlexItemTypes))) {
                try {
                    ImageCache.Add(plexType, "");
                } catch {
                    //throw; 
                }
            }
        }

        private static object sync = new object();

        public static string GetArtWork(string imageOnlinePath) {
            if (String.IsNullOrEmpty(imageOnlinePath)) {
                return string.Empty;
            }
            if (File.Exists(imageOnlinePath)) {
                return imageOnlinePath;
            }
            string localImagePath = DefaulIconPath;
            if (!Directory.Exists(localImagePath)) {
                Directory.CreateDirectory(localImagePath);
            }
            localImagePath = Path.Combine(localImagePath, GetSafeFilenameFromUrl(imageOnlinePath, '_'));
            if (File.Exists(localImagePath)) {
                return localImagePath;
            }
            lock (sync) {
                if (!ImageCache.ContainsKey(imageOnlinePath)) {
                    ImageCache.Add(imageOnlinePath, localImagePath);
                    try {
                        DownloadImage(imageOnlinePath, localImagePath);
                    } catch {
                        //ToDo
                    }
                }
                return ImageCache[imageOnlinePath];

            }
        }

        public static void DownloadImage(string imageOnlinePath, string imageLocalPath) {
            WebClient ArtWorkRetriever = new WebClient();
            PlexInterface.PlexServerCurrent.AddAuthHeaders(ref ArtWorkRetriever);
            ArtWorkRetriever.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(ArtWorkRetriever_DownloadFileCompleted);
            ArtWorkRetriever.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ArtWorkRetriever_DownloadProgressChanged);

            ArtWorkRetriever.DownloadFileAsync(new Uri(PlexInterface.PlexServerCurrent.UriPlexBase, imageOnlinePath), imageLocalPath, imageOnlinePath);
        }

        static void ArtWorkRetriever_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
            if (e.Error != null || e.Cancelled) {
                //ToDo handle
                return;
            } else {
                if (e.UserState is string) {
                    string artWorkIndex = (string)e.UserState;
                    if (ImageCache.ContainsKey(artWorkIndex)) {
                        OnArtWorkRetrieved(ImageCache[artWorkIndex]);
                    }
                }
            }
        }

        static void ArtWorkRetriever_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            //throw new NotImplementedException();
        }

        private static string GetSafeFilenameFromUrl(string url, char replaceChar) {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars()) {
                url = url.Replace(c, replaceChar);
            }
            return url;
        }

    }
}
