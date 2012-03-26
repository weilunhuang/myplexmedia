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
using System.Collections.Generic;
using PlexMediaCenter.Util;

namespace PlexMediaCenter.Plex.Connection {
    [Serializable]
    public class PlexServer : IEquatable<PlexServer> {

        public PlexServer() {
        }

        public String MachineIdentifier { get; private set; }
        private List<BaseConnectionInfo> KnownConnections { get; set; }

        public PlexServer(string machineIdentifier) {
            MachineIdentifier = machineIdentifier;
            KnownConnections = new List<BaseConnectionInfo>();
        }

        public void AddConnectionInfo(BaseConnectionInfo connectionInfo) {
            KnownConnections.Add(connectionInfo);
        }

        public String FriendlyName {
            get { return ServerCapabilities != null ? ServerCapabilities.FriendlyName : String.Empty; }
        }


        [XmlIgnore]
        public bool IsBonjour { get; set; }

        [XmlIgnore]
        public bool IsMyPlex { get; set; }

        [XmlIgnore]
        public bool IsOnline { get; set; }

        
        public static string EncryptPassword(string userName, string userPass) {
            return Encryption.GetSHA1Hash(userName.ToLower() + Encryption.GetSHA1Hash(userPass));
        }

        public override string ToString() {
            return String.Format("{0} @ {1} [{2}]", UserName, FriendlyName, UriPlexBase.Host);
        }

        internal bool Authenticate(ref WebClient webClient) {
            return true;
        }
        #region IEquatable<PlexServer> Members

        public bool Equals(PlexServer other) {
            return MachineIdentifier.Equals(other.MachineIdentifier);
        }
        #endregion
    }
}