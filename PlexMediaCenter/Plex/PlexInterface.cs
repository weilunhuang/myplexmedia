﻿using System;
using System.Collections.Generic;
using System.Net;
using PlexMediaCenter.Plex.Connection;
using PlexMediaCenter.Util;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Plex.Data;

namespace PlexMediaCenter.Plex {
    public static class PlexInterface {

        private static WebClient _webClient;
        private static PlexAuthenticationProxy _authProxy;
        public static bool IsConnected { get; private set; }
        public static bool IsBusy { get { return _webClient.IsBusy; } }

        public static event OnResponseProgressEventHandler OnResponseProgress;
        public delegate void OnResponseProgressEventHandler(int progress);
        public static event OnResponseReceivedEventHandler OnResponseReceived;
        public delegate void OnResponseReceivedEventHandler(MediaContainer response);
        public static event OnPlexErrorEventHandler OnPlexError;
        public delegate void OnPlexErrorEventHandler(PlexException e);

        public static ServerManager ServerManager { get; private set; }
        public static ArtworkRetriever ArtworkRetriever { get; private set; }

        static PlexInterface() {
            _webClient = new WebClient();
            _webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(_webClient_DownloadDataCompleted);
            _webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(_webClient_DownloadProgressChanged);
            _authProxy = new PlexAuthenticationProxy();
        }

        public static void Init(string serverListXmlPath, string defaultBasePath, string defaultImage) {
            ServerManager = new ServerManager(serverListXmlPath);
            ServerManager.OnServerManangerError += new ServerManager.OnServerManangerErrorEventHandler(ServerManager_OnServerManangerError);
            ArtworkRetriever = new ArtworkRetriever(defaultBasePath, defaultImage);
            ArtworkRetriever.OnArtworkRetrievalError += new ArtworkRetriever.OnArtworkRetrievalErrorEventHandler(MediaRetrieval_OnArtWorkRetrievalError);
        }

        static void MediaRetrieval_OnArtWorkRetrievalError(PlexException e) {
            OnPlexError(e);
        }

        static void ServerManager_OnServerManangerError(PlexException e) {
            OnPlexError(e);
        }

        static void _webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            OnResponseProgress(e.ProgressPercentage);
            Console.WriteLine(e.ProgressPercentage + " (" + e.BytesReceived + " / " + e.TotalBytesToReceive + ")");
        }

        static void _webClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {
            OnResponseReceived(Serialization.DeSerializeXML<MediaContainer>(System.Text.ASCIIEncoding.Default.GetString(e.Result)));
        }

        public static bool Login(PlexServer plexServer) {
            if (String.IsNullOrEmpty(plexServer.HostAdress)) {
                return false;
            } else {
                return ServerManager.Authenticate(ref _webClient, plexServer);
            }
        }

        public static MediaContainer TryGetPlexSections(PlexServer plexServer) {
            if (plexServer != null && Login(plexServer)) {
                try {
                    ServerManager.SetCurrentPlexServer(plexServer);
                    return RequestPlexItems(plexServer.UriPlexSections);
                } catch (Exception e) {
                    OnPlexError(new PlexException(typeof(PlexInterface), "TryGetPlexSections failed for: " + plexServer.ToString(), e));
                }
            } else {
                OnPlexError(new PlexException(typeof(PlexInterface), "Unable to login to: " + plexServer.ToString(), null));
            }
            return null;
        }

        public static MediaContainer TryGetPlexServerRoot(PlexServer plexServer) {
            if (Login(plexServer)) {
                try {
                    ServerManager.SetCurrentPlexServer(plexServer);
                    return RequestPlexItems(plexServer.UriPlexBase);
                } catch (Exception e) {
                    OnPlexError(new PlexException(typeof(PlexInterface), "Unable to get root items for: " + plexServer.ToString(), e));
                }
            } else {
                OnPlexError(new PlexException(typeof(PlexInterface), "Unable to login to: " + plexServer.ToString(), null));
            }
            return null;
        }

        public static MediaContainer RequestPlexItems(Uri selectedPath) {
            try {
                MediaContainer requestedContainer = Serialization.DeSerializeXML<MediaContainer>(_webClient.DownloadString(selectedPath));
                requestedContainer.UriSource = selectedPath;
                return requestedContainer;
            } catch (Exception e) {
                OnPlexError(new PlexException(typeof(PlexInterface), "Unable to get items from path: " + selectedPath.AbsoluteUri, e));
            }
            return null;
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

        public static string GetPlayBackProxyUrl(string sourceUrl) {
            if (!_authProxy.Started) {
                _authProxy.Start();
            }
            return _authProxy.GetProxyUrl(sourceUrl);
        }

        public static PlexServer PlexServerCurrent { get { return ServerManager.PlexServerCurrent; } }

        public static bool Is3GConnected { get; set; }

        public static bool IsLANConnected { get { return PlexServerCurrent.IsBonjour; } }

        public static bool ShouldTranscode { get; set; }
    }
}
