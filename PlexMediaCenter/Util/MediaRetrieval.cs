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
            //ImageCache = new Dictionary<string, string>();
            //foreach (string plexType in Enum.GetNames(typeof(EPlexItemTypes))) {
            //    try {
            //        ImageCache.Add(plexType, Image.FromFile(String.Format(@"\Resources\{0}.png", plexType)));
            //    } catch {
            //        //throw; 
            //    }
            //}
        }

        private static object sync = new object();

        public static string GetArtWork(string imageOnlinePath) {
            lock (sync) {
                if (String.IsNullOrEmpty(imageOnlinePath)) {
                    return DefaulIconPath;
                } else if (!ImageCache.ContainsKey(imageOnlinePath)) {
                    ImageCache.Add(imageOnlinePath, DefaulIconPath);
                    try {
                        DownloadImage(imageOnlinePath);
                    } catch {
                        //ToDo
                    }
                }
                return ImageCache[imageOnlinePath];
            }
        }

        public static void DownloadImage(string imageOnlinePath) {
            WebClient ArtWorkRetriever = new WebClient();
            PlexInterface.PlexServerCurrent.AddAuthHeaders(ref ArtWorkRetriever);
            ArtWorkRetriever.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(ArtWorkRetriever_DownloadFileCompleted);
            ArtWorkRetriever.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ArtWorkRetriever_DownloadProgressChanged);
            ArtWorkRetriever.DownloadFileAsync(new Uri(PlexInterface.PlexServerCurrent.UriPlexBase, imageOnlinePath), DefaulIconPath);
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

    }
}
