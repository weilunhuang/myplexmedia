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
    public class PlexServer : IEquatable<PlexServer> {
        private const int PlexPort = 32400;

        public PlexServer() {
        }

        public PlexServer(string hostName, string hostAdress, string userName, string userPass) {
            HostName = hostName;
            HostAdress = hostAdress;
            UserName = userName;
            EncryptPassword(userName, userPass);
        }

        public PlexServer(string hostName, string hostAdress) {
            HostName = hostName;
            HostAdress = hostAdress;
        }

        public String FriendlyName {
            get { return ServerCapabilities != null ? ServerCapabilities.FriendlyName : String.Empty; }
        }

        public String HostName { get; set; }
        public String HostAdress { get; set; }
        public String UserName { get; set; }
        public String UserPass { get; set; }

        [XmlIgnore]
        public PlexCapabilitiesServer ServerCapabilities { get; set; }

        [XmlIgnore]
        public bool IsBonjour { get; set; }

        [XmlIgnore]
        public bool IsOnline { get; set; }

        [XmlIgnore]
        public Uri UriPlexSections {
            get { return new Uri(UriPlexBase, "/library/sections/"); }
        }

        [XmlIgnore]
        public Uri UriPlexBase {
            get { return new UriBuilder("http", HostAdress, PlexPort, "").Uri; }
        }

        #region IEquatable<PlexServer> Members

        public bool Equals(PlexServer other) {
            return HostAdress.Equals(other.HostAdress);
        }

        #endregion

        public void EncryptPassword(string userName, string userPass) {
            UserPass = Encryption.GetSHA1Hash(userName.ToLower() + Encryption.GetSHA1Hash(userPass));
        }

        public override string ToString() {
            return String.Format("{0}@{1}", UserName, UriPlexBase.Host);
        }

        public void AddAuthHeaders(ref WebClient webClient) {
            webClient.Headers["X-Plex-User"] = UserName;
            webClient.Headers["X-Plex-Pass"] = UserPass;
        }

        internal bool Authenticate(ref WebClient webClient) {
            if (CheckSocketConnection()) {
                webClient.Headers["X-Plex-User"] = UserName;
                webClient.Headers["X-Plex-Pass"] = UserPass;
                try {
                    string serverXmlResponse = webClient.DownloadString(UriPlexBase);
                    ServerCapabilities =
                        GetServerCapabilities(Serialization.DeSerializeXML<MediaContainer>(serverXmlResponse));
                    IsOnline = true;
                    return ServerCapabilities != null;
                } catch (Exception e) {
                }
            }
            IsOnline = false;
            return false;
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

        private static PlexCapabilitiesServer GetServerCapabilities(MediaContainer serverResponse) {
            return serverResponse == null ? null : new PlexCapabilitiesServer(serverResponse);
        }
    }
}