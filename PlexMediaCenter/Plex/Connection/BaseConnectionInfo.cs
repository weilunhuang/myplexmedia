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
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using PlexMediaCenter.Plex.Data.Types;
using PlexMediaCenter.Util;

namespace PlexMediaCenter.Plex.Connection {
    [Serializable]
    public abstract class BaseConnectionInfo {

        public String MachineIdentifier { get; set; }
        public String HostName { get; set; }
        public String HostAdress { get; set; }
        public int PlexPort { get; set; }

        [XmlIgnore]
        public bool IsOnline { get; set; }

        [XmlIgnore]
        public Uri UriPlexBase {
            get { return new UriBuilder("http", HostAdress, PlexPort, "").Uri; }
        }

        public PlexCapabilitiesServer Capabilities { get; set; }

        public BaseConnectionInfo() {
        }

        public BaseConnectionInfo(string hostName, string hostAdress, int plexPort) {
            HostName = hostName;
            HostAdress = hostAdress;
            PlexPort = plexPort;
        }

        internal abstract void AddAuthHeaders(ref WebClient webClient);

        internal abstract string GetAuthUrlParameters();

        public bool TryConnect(ref WebClient webClient) {
            if (CheckSocketConnection()) {
                AddAuthHeaders(ref webClient);
                try {
                    string serverXmlResponse = webClient.DownloadString(UriPlexBase);
                    Capabilities = GetServerCapabilities(Serialization.DeSerializeXML<MediaContainer>(serverXmlResponse));
                    MachineIdentifier = Capabilities.MachineIdentifier;
                    IsOnline = true;
                    return true;
                } catch (Exception e) {
                    IsOnline = false;
                }
            }
            return IsOnline;
        }

        private PlexCapabilitiesServer GetServerCapabilities(MediaContainer serverResponse) {
            return serverResponse == null ? null : new PlexCapabilitiesServer(serverResponse);
        }

        private bool CheckSocketConnection() {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) {
                try {
                    IAsyncResult result = socket.BeginConnect(HostAdress, PlexPort, null, null);
                    return result.AsyncWaitHandle.WaitOne(2000, true);
                } catch {
                    socket.Close();
                    return false;
                }
            }
        }


    }
}