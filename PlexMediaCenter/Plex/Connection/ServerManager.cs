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
using System.IO;
using System.Linq;
using PlexMediaCenter.Util;

namespace PlexMediaCenter.Plex.Connection {
    public class ServerManager {
        #region Delegates

        public delegate void OnPlexServersChangedEventHandler(List<PlexServer> updatedServerList);

        public delegate void OnServerManangerErrorEventHandler(PlexException e);

        #endregion

        private PlexServer _plexServerCurrent;

        public ServerManager(string serverXmlFile) {
            if (string.IsNullOrEmpty(serverXmlFile)) {
                throw new Exception();
            }
            ServerXmlFile = serverXmlFile;
            PlexServers = LoadPlexServers();
            BonjourDiscovery.OnBonjourServer += BonjourDiscovery_OnBonjourServer;
        }

        private string ServerXmlFile { get; set; }

        public List<PlexServer> PlexServers { get; private set; }

        public PlexServer PlexServerCurrent {
            get { return _plexServerCurrent; }
            private set {
                _plexServerCurrent = value;
                if (!PlexServers.Contains(value)) {
                    PlexServers.Insert(0, value);
                }
                SavePlexServers(PlexServers);
            }
        }

        public event OnServerManangerErrorEventHandler OnServerManangerError;
        public event OnPlexServersChangedEventHandler OnPlexServersChanged;

        ~ServerManager() {
            SavePlexServers(PlexServers);
        }

        private List<PlexServer> LoadPlexServers() {
            if (File.Exists(ServerXmlFile)) {
                try {
                    return Serialization.DeSerialize<List<PlexServer>>(ServerXmlFile);
                } catch (Exception e) {
                    OnServerManangerError(new PlexException(GetType(),
                                                            String.Format("Unable to deserialize '{0}'", ServerXmlFile),
                                                            e));
                }
            }
            return new List<PlexServer>();
        }

        public void SavePlexServers(List<PlexServer> plexServers) {
            if (plexServers == null) {
                OnServerManangerError(new PlexException(GetType(), "Unable to save server list",
                                                        new ArgumentNullException("plexServers")));
                return;
            }
            try {
                if (File.Exists(ServerXmlFile)) {
                    File.Delete(ServerXmlFile);
                }
                Serialization.Serialize(ServerXmlFile, plexServers);
            } catch (Exception e) {
                OnServerManangerError(new PlexException(GetType(),
                                                        String.Format("Unable to serialize '{0}'", ServerXmlFile), e));
            }
        }

        public PlexServer TryFindPlexServer(Uri plexUrl) {
            PlexServer server = PlexInterface.ServerManager.PlexServers.Find(svr => svr.HostAdress.Equals(plexUrl.Host));
            if (server != null) {
                return server;
            } else {
                PlexException pe = new PlexException(typeof (ServerManager),
                                                     "Couldn't find PlexServer in List: " + plexUrl.Host,
                                                     new KeyNotFoundException());
                OnServerManangerError(pe);
                throw pe;
            }
        }

        public void SetCurrentPlexServer(PlexServer server) {
            PlexServerCurrent = server;
        }

        private void BonjourDiscovery_OnBonjourServer(PlexServer bonjourDiscoveredServer) {
            if (PlexServers.Contains<PlexServer>(bonjourDiscoveredServer)) {
                PlexServers.Find(x => x.Equals(bonjourDiscoveredServer)).IsBonjour = true;
            } else {
                PlexServers.Add(bonjourDiscoveredServer);
                OnPlexServersChanged(PlexServers);
            }
        }

        public void RefrehBonjourServers() {
            if (PlexServers.Count > 0) {
                OnPlexServersChanged(PlexServers);
            }
            BonjourDiscovery.RefreshBonjourDiscovery();
        }

        internal bool Authenticate(ref System.Net.WebClient _webClient, PlexServer plexServer) {
            return plexServer.Authenticate(ref _webClient);
        }
    }
}