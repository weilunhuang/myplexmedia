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
using System.Runtime.InteropServices;

namespace PlexMediaCenter.Plex {
    public static class PlexInterface {
        #region Events & Delegates

        public delegate void OnPlexErrorEventHandler(PlexException e);
        public delegate void OnResponseProgressEventHandler(object userToken, int progress);
        public delegate void OnResponseReceivedEventHandler(object userToken, MediaContainer response);

        public static event OnResponseProgressEventHandler OnResponseProgress = delegate { };
        public static event OnResponseReceivedEventHandler OnResponseReceived = delegate { };
        public static event OnPlexErrorEventHandler OnPlexError = delegate { };

        #endregion

        #region Properties

        private static WebClient PlexWebClient;
        public static MyPlex MyPlex { get; private set; }
        public static ServerManager ServerManager { get; private set; }
        public static ArtworkRetriever ArtworkRetriever { get; private set; }

        #endregion

        #region Initialization

        public static void Init(string serverListXmlPath, string defaultBasePath, string defaultImage, WebClient webClient = default(WebClient)) {
            if (webClient == null) {
                webClient = new WebClient();
            }
            PlexWebClient = webClient;
            PlexWebClient.DownloadDataCompleted += _webClient_DownloadDataCompleted;
            PlexWebClient.DownloadProgressChanged += _webClient_DownloadProgressChanged;

            ServerManager = new ServerManager(ref PlexWebClient,serverListXmlPath);
            ServerManager.OnServerManangerError += ServerManager_OnServerManangerError;

            ArtworkRetriever = new ArtworkRetriever(defaultBasePath, defaultBasePath);
            ArtworkRetriever.OnArtworkRetrievalError += MediaRetrieval_OnArtWorkRetrievalError;
        }

        public static bool MyPlexLogin(string user, string pass) {
            MyPlex = new MyPlex(new NetworkCredential(user, pass));
            if (MyPlex.Authenticate(ref PlexWebClient) && ServerManager != null) {
                ServerManager.AddMyPlexServers(MyPlex.MyPlexServers);
            } else {
                return false;
            }
            return true;
        }

        #endregion

        #region Plex Server Requests

        public static PlexServer GetPlexServerFromUri(Uri sourcePath) {
            return ServerManager.PlexServers.Single(svr => 
                svr.KnownConnections.Any(con => 
                    con.Value.UriPlexBase.IsBaseOf(sourcePath)));
        }

        public static MediaContainer TryGetPlexSections(PlexServer plexServer) {
            if (plexServer != null && plexServer.Authenticate(ref PlexWebClient)) {
                try {
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

        public static MediaContainer RequestPlexItems(Uri selectedPath) {
            try {
                MediaContainer requestedContainer =
                    Serialization.DeSerializeXML<MediaContainer>(PlexWebClient.DownloadString(selectedPath));
                requestedContainer.UriSource = selectedPath;
                return requestedContainer;
            } catch (Exception e) {
                OnPlexError(new PlexException(typeof(PlexInterface),
                                              "Unable to get items from path: " + selectedPath.AbsoluteUri, e));
            }
            return null;
        }

        public static void RequestPlexItemsAsync(Uri path, object userToken) {
            if (PlexWebClient.IsBusy) {
                OnPlexError(new PlexException(typeof(PlexInterface), "Another Request in progress!", null));
                return;
            }
            OnResponseProgress(userToken, 0);
            PlexWebClient.DownloadDataAsync(path, userToken);
        }

        public static void RequestPlexItemsCancel() {
            if (PlexWebClient.IsBusy) {
                PlexWebClient.CancelAsync();
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

        private static void ServerManager_OnServerManangerError(PlexException e) {
            OnPlexError(e);
        }

        #endregion
    }
}