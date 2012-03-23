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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using PlexMediaCenter.Plex.Connection;
using PlexMediaCenter.Plex.Data;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Util;

namespace PlexMediaCenter.Plex {
    public static class PlexInterface {
        #region Delegates

        public delegate void OnPlexErrorEventHandler(PlexException e);

        public delegate void OnResponseProgressEventHandler(object userToken, int progress);

        public delegate void OnResponseReceivedEventHandler(object userToken, MediaContainer response);

        #endregion

        private static WebClient _webClient;

        static PlexInterface() {
            _webClient = new WebClient();
            _webClient.DownloadDataCompleted += _webClient_DownloadDataCompleted;
            _webClient.DownloadProgressChanged += _webClient_DownloadProgressChanged;
        }

        public static bool IsConnected { get; private set; }

        public static bool IsBusy {
            get { return _webClient.IsBusy; }
        }

        public static MyPlex MyPlex { get; private set; }
        public static ServerManager ServerManager { get; private set; }
        public static ArtworkRetriever ArtworkRetriever { get; private set; }

        public static PlexServer PlexServerCurrent {
            get { return ServerManager.PlexServerCurrent; }
        }

        public static bool Is3GConnected { get; set; }

        public static bool IsLANConnected {
            get { return PlexServerCurrent.IsBonjour; }
        }

        public static bool ShouldTranscode { get; set; }
        public static event OnResponseProgressEventHandler OnResponseProgress;
        public static event OnResponseReceivedEventHandler OnResponseReceived;
        public static event OnPlexErrorEventHandler OnPlexError;

        public static void Init(string serverListXmlPath, string defaultBasePath, string defaultImage) {
            ServerManager = new ServerManager(serverListXmlPath);
            ServerManager.OnServerManangerError += ServerManager_OnServerManangerError;
            ArtworkRetriever = new ArtworkRetriever(defaultBasePath, defaultImage);
            ArtworkRetriever.OnArtworkRetrievalError += MediaRetrieval_OnArtWorkRetrievalError;
        }

        public static bool MyPlexLogin(string user, string pass) {
            MyPlex = new MyPlex(new NetworkCredential(user, pass));
            if (MyPlex.Authenticate(ref _webClient) && ServerManager != null) {
                ServerManager.AddMyPlexServerList(MyPlex);
            } else {
                return false;
            }
            return true;
        }

        #region Plex Server Requests

        public static MediaContainer TryGetPlexSections(PlexServer plexServer) {
            if (plexServer != null && Login(plexServer)) {
                try {
                    ServerManager.SetCurrentPlexServer(plexServer);
                    System.Threading.Thread.Sleep(500);
                    return RequestPlexItems(plexServer.UriPlexSections);
                } catch (Exception e) {
                    OnPlexError(new PlexException(typeof(PlexInterface), "TryGetPlexSections failed for: " + plexServer,
                                                  e));
                }
            } else {
                OnPlexError(new PlexException(typeof(PlexInterface),
                                              string.Format("Unable to login to: ", plexServer.ToString()), null));
            }
            return null;
        }

        public static MediaContainer TryGetPlexServerRoot(PlexServer plexServer) {
            if (Login(plexServer)) {
                try {
                    ServerManager.SetCurrentPlexServer(plexServer);
                    return RequestPlexItems(plexServer.UriPlexBase);
                } catch (Exception e) {
                    OnPlexError(new PlexException(typeof(PlexInterface), "Unable to get root items for: " + plexServer,
                                                  e));
                }
            } else {
                OnPlexError(new PlexException(typeof(PlexInterface), "Unable to login to: " + plexServer, null));
            }
            return null;
        }

        public static MediaContainer RequestPlexItems(Uri selectedPath) {
            try {
                MediaContainer requestedContainer =
                    Serialization.DeSerializeXML<MediaContainer>(_webClient.DownloadString(selectedPath));
                requestedContainer.UriSource = selectedPath;
                return requestedContainer;
            } catch (Exception e) {
                OnPlexError(new PlexException(typeof(PlexInterface),
                                              "Unable to get items from path: " + selectedPath.AbsoluteUri, e));
            }
            return null;
        }

        public static void RequestPlexItemsAsync(Uri path, object userToken) {
            if (_webClient.IsBusy) {
                OnPlexError(new PlexException(typeof(PlexInterface), "Another Request in progress!", null));
                return;
            }
            OnResponseProgress(userToken, 0);
            _webClient.DownloadDataAsync(path, userToken);
        }

        public static void RequestPlexItemsCancel() {
            if (_webClient.IsBusy) {
                _webClient.CancelAsync();
            }
        }

        private static void _webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            OnResponseProgress(e.UserState, e.ProgressPercentage);
        }

        private static void _webClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {
            if (e.Error != null) {
                OnPlexError(new PlexException(typeof(PlexInterface), e.Error.Message, null));
            } else {
                OnResponseReceived(e.UserState,
                                   Serialization.DeSerializeXML<MediaContainer>(
                                       System.Text.Encoding.Default.GetString(e.Result)));
            }
        }

        #endregion

        #region ArtWorkRetriever

        private static void MediaRetrieval_OnArtWorkRetrievalError(PlexException e) {
            OnPlexError(e);
        }

        #endregion

        #region Transcoding

        public static IEnumerable<string> GetAllVideoPartKeys(MediaContainerVideo videoContainer) {
            return from media in videoContainer.Media from part in media.Part select part.key;
        }

        #endregion

        #region ServerManager

        public static bool PlexServersAvailable {
            get {
                return ServerManager.PlexServers != null
                       && ServerManager.PlexServers.Count > 0
                       && ServerManager.PlexServerCurrent != null;
            }
        }

        private static void ServerManager_OnServerManangerError(PlexException e) {
            OnPlexError(e);
        }

        public static bool Login(PlexServer plexServer) {
            return !String.IsNullOrEmpty(plexServer.HostAdress) &&
                   ServerManager.Authenticate(ref _webClient, plexServer);
        }

        public static void RefreshBonjourServers() {
            ServerManager.RefrehBonjourServers();
        }

        #endregion
    }
}