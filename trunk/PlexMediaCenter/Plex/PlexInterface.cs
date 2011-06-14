using System;
using System.Collections.Generic;
using System.Net;
using PlexMediaCenter.Plex.Connection;
using PlexMediaCenter.Util;
using PlexMediaCenter.Plex.Data.Types;

namespace PlexMediaCenter.Plex {
   public static class PlexInterface {

        private static WebClient _webClient;
        public static bool IsConnected { get; private set; }
        public static bool IsBusy { get { return _webClient.IsBusy; } }

        public static event OnResponseProgressEventHandler OnResponseProgress;
        public delegate void OnResponseProgressEventHandler(int progress);
        public static event OnResponseReceivedEventHandler OnResponseReceived;
        public delegate void OnResponseReceivedEventHandler(MediaContainer response);
        public static event OnPlexErrorEventHandler OnPlexError;
        public delegate void OnPlexErrorEventHandler(Exception e);        

        private static ServerManager ServerManager { get; set; }

        static PlexInterface() {
            _webClient = new WebClient();
            _webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(_webClient_DownloadDataCompleted);
            _webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(_webClient_DownloadProgressChanged);
        }               

       public static void Init(string serverListXmlPath, string defaultIconPath){
           ServerManager = new ServerManager(serverListXmlPath);          
           MediaRetrieval.DefaulIconPath = defaultIconPath;
       }

        static void _webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            OnResponseProgress(e.ProgressPercentage);
            Console.WriteLine(e.ProgressPercentage + " (" + e.BytesReceived + " / " + e.TotalBytesToReceive + ")");
        }

        static void _webClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {            
            OnResponseReceived(Serialization.DeSerializeXML<MediaContainer>(System.Text.ASCIIEncoding.Default.GetString(e.Result)));                
        }               

        public static bool Login(PlexServer plexServer) {
           return ServerManager.Authenticate(ref _webClient, plexServer);            
        }

        public static MediaContainer TryGetPlexSections() {
            return TryGetPlexSections(PlexServerCurrent);
        }

        public static MediaContainer TryGetPlexSections(PlexServer plexServer) {
            if (Login(plexServer)) {
                try {
                    ServerManager.SetPlexServer(plexServer);
                    return RequestPlexItems(plexServer.UriPlexSections);
                } catch (Exception e) {
                    throw e;
                }
            } else {                
                return null;
            }
        }

        public static MediaContainer TryGetPlexServerRoot(PlexServer plexServer) {
            if (Login(plexServer)) {
                try {
                    ServerManager.SetPlexServer(plexServer);
                    return RequestPlexItems(plexServer.UriPlexBase);
                } catch (Exception e) {
                    throw e;
                }
            } else {
                OnPlexError(new Exception("Unable to login to:" + plexServer.HostAdress));
                return null;
            }
        }

        public static MediaContainer RequestPlexItems(Uri selectedPath) {   
            try {              
                MediaContainer requestedContainer = Serialization.DeSerializeXML<MediaContainer>(_webClient.DownloadString(selectedPath));                
                requestedContainer.UriSource = selectedPath;
                return requestedContainer;
            } catch (Exception e) {
                OnPlexError(e);
                throw e;               
            }
        }

        public static MediaContainer RequestPlexItems(string localPath){
            try {
                MediaContainer requestedContainer = Serialization.DeSerialize<MediaContainer>(localPath);
                requestedContainer.UriSource = new Uri(localPath);
                return requestedContainer;
            } catch (Exception e) {
                OnPlexError(e);
                throw e;
            }
        }        

        public static bool PlexServersAvailable {
            get {
                return ServerManager.PlexServers != null
                    && ServerManager.PlexServers.Count > 0
                    && ServerManager.PlexServerCurrent != null;
            }
        }

        public static void RefreshBonjourServers() {
            ServerManager.RefrehBonjourServers();
        }

        public static void RequestPlexItemsAsync(Uri path) {
            _webClient.DownloadDataAsync(path);
        }

        public static IEnumerable<string> GetAllVideoPartKeys(MediaContainerVideo videoContainer) {
            foreach (Media media in videoContainer.Media) {                
                foreach (MediaPart part in media.Part) {
                    yield return part.key;
                }
            }
        }

        public static PlexServer PlexServerCurrent { get { return ServerManager.PlexServerCurrent; } }

        public static bool Is3GConnected { get; set; }

        public static bool IsLANConnected { get { return PlexServerCurrent.IsBonjour; } }

        public static bool ShouldTranscode { get; set; }
    }
}
