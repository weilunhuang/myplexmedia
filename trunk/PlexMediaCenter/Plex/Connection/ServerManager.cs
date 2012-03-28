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
using System.Net;
using System.Text.RegularExpressions;

namespace PlexMediaCenter.Plex.Connection {
    public class ServerManager {
        #region Delegates

        public delegate void OnPlexServersChangedEventHandler(List<PlexServer> updatedServerList);

        internal delegate void OnServerManangerErrorEventHandler(PlexException e);

        #endregion
        private WebClient WebClient;
        private PlexServer CurrentPlexServer { get; set; }

        public ServerManager(ref WebClient webClient, string serverXmlFile) {
            if (string.IsNullOrEmpty(serverXmlFile)) {
                throw new Exception();
            }
            WebClient = webClient;
            ServerXmlFile = serverXmlFile;
            PlexServers = LoadPlexServers();
            BonjourDiscovery.OnBonjourConnection += BonjourDiscovery_OnBonjourConnection;
            BonjourDiscovery.StartBonjourDiscovery();
        }

        private string ServerXmlFile { get; set; }

        public List<PlexServer> PlexServers { get; private set; }

        public PlexServer PlexServerCurrent {
            get { return PlexServerCurrent; }
            private set {
                PlexServerCurrent = value;
                if (!PlexServers.Contains(value)) {
                    PlexServers.Insert(0, value);
                    SavePlexServers(PlexServers);
                }
            }
        }

        internal static event OnServerManangerErrorEventHandler OnServerManangerError;
        public event OnPlexServersChangedEventHandler OnPlexServersChanged;

        ~ServerManager() {
            SavePlexServers(PlexServers);
        }

        private List<PlexServer> LoadPlexServers() {
            if (File.Exists(ServerXmlFile)) {
                try {
                    return Serialization.DeSerialize<List<PlexServer>>(ServerXmlFile);
                } catch (Exception e) {
                    if (OnServerManangerError != null) {
                        OnServerManangerError(new PlexException(GetType(),
                                                                String.Format("Unable to deserialize '{0}'", ServerXmlFile),
                                                                e));
                    }
                }
            }
            return new List<PlexServer>();
        }

        private void SavePlexServers(List<PlexServer> plexServers) {
            if (plexServers == null) {
                OnServerManangerError(new PlexException(GetType(), "Unable to save server list",
                                                        new ArgumentNullException("plexServers")));
                return;
            }
            try {
                if (File.Exists(ServerXmlFile)) {
                    File.Delete(ServerXmlFile);
                }
                Serialization.Serialize(ServerXmlFile, plexServers.Where(svr => !svr.IsMyPlex).ToList());
            } catch (Exception e) {
                OnServerManangerError(new PlexException(GetType(),
                                                        String.Format("Unable to serialize '{0}'", ServerXmlFile), e));
            }
        }

        public PlexServer TryFindPlexServer(Uri plexUrl) {
            PlexServer server = PlexInterface.ServerManager.PlexServers.Find(svr => svr.UriPlexBase.Host.Equals(plexUrl.Host));
            if (server != null) {
                return server;
            } else {
                PlexException pe = new PlexException(typeof(ServerManager),
                                                     "Couldn't find PlexServer in List: " + plexUrl.Host,
                                                     new KeyNotFoundException());
                OnServerManangerError(pe);
                throw pe;
            }
        }

        public void SetCurrentPlexServer(PlexServer server) {
            PlexServerCurrent = server;
        }

        private void BonjourDiscovery_OnBonjourConnection(BonjourConnectionInfo bonjourDiscoveredServer) {
            try {
                string machineIdentifier = string.Empty;
                if (bonjourDiscoveredServer.TryConnect(ref WebClient, ref machineIdentifier)) {
                    if (PlexServers.Exists(svr => svr.MachineIdentifier.Equals(machineIdentifier))) {
                        PlexServers.Single(svr => svr.MachineIdentifier.Equals(machineIdentifier)).AddConnectionInfo(bonjourDiscoveredServer);
                    } else {
                        PlexServers.Add(new PlexServer(machineIdentifier, bonjourDiscoveredServer));
                    }
                    OnPlexServersChanged(PlexServers);
                }
            } catch {

            }
        }



        internal void AddMyPlexServers(List<MyPlexConnectionInfo> myPlexServerConnections) {
            foreach (MyPlexConnectionInfo conn in myPlexServerConnections) {
                string machine = string.Empty;
                conn.TryConnect(ref WebClient, ref machine); 
                if (PlexServers.Exists(svr => svr.MachineIdentifier.Equals(conn.MachineIdentifier))) {
                    PlexServers.Single(svr => svr.MachineIdentifier.Equals(conn.MachineIdentifier)).AddConnectionInfo(conn);
                } else {
                    PlexServers.Add(new PlexServer(conn.MachineIdentifier, conn));
                }
            }
            OnPlexServersChanged(PlexServers);
        }

        public PlexServer AddManualServerConnection(ManualConnectionInfo manualServerConnection) {
            PlexServer tmpServer = null;
            string machine = string.Empty;
            if(manualServerConnection.TryConnect(ref WebClient, ref machine) && PlexServers.Exists(svr => svr.MachineIdentifier.Equals(manualServerConnection.MachineIdentifier))) {
                tmpServer = PlexServers.Single(svr => svr.MachineIdentifier.Equals(manualServerConnection.MachineIdentifier));
                tmpServer.AddConnectionInfo(manualServerConnection);
            } else {
                tmpServer = new PlexServer(manualServerConnection.MachineIdentifier, manualServerConnection);
                PlexServers.Add(tmpServer);
            }
            return tmpServer;
        }

        public void RefrehBonjourServers() {
            if (PlexServers.Count > 0) {
                OnPlexServersChanged(PlexServers);
            }
            BonjourDiscovery.RefreshBonjourDiscovery();
        }

        internal bool Authenticate(PlexServer plexServer) {
            return plexServer.Authenticate(ref WebClient);
        }
    }
}