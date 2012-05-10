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
using System.Linq;
using PlexMediaCenter.Util;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace PlexMediaCenter.Plex.Connection {
   [DataContract]
    public class PlexServer : IEquatable<PlexServer> {

        public string MachineIdentifier { get; set; }

        public PlexServer() {
        }

        internal PlexServer(string machineIdentifier, BaseConnectionInfo connectionInfo) {
            MachineIdentifier = machineIdentifier;
            AddConnectionInfo(connectionInfo);
        }

        [XmlIgnore]
        public string FriendlyName { get { return CurrentConnection.Capabilities != null ? CurrentConnection.Capabilities.FriendlyName : CurrentConnection.HostName; } }
        [XmlIgnore]
        public string PlexVersion { get { return CurrentConnection.Capabilities != null ? CurrentConnection.Capabilities.PMSVersion : String.Empty; } }

        public SerializableDictionary<Type, BaseConnectionInfo> KnownConnections { get; set; }

        [XmlIgnore]
        public Uri UriPlexBase { get { return CurrentConnection.UriPlexBase; } }

        public Uri UriPlexSections {
            get { return new Uri(CurrentConnection.UriPlexBase, "/library/sections/"); }
        }

        [XmlIgnore]
        public BaseConnectionInfo CurrentConnection {
            get {
                if (IsBonjour) {
                    return KnownConnections[typeof(BonjourConnectionInfo)];
                }
                if (IsMyPlex) {
                    return KnownConnections[typeof(MyPlexConnectionInfo)];
                }
                if (IsManual) {
                    return KnownConnections[typeof(ManualConnectionInfo)];
                } else {
                   return KnownConnections.First().Value;
                }
            }
        }

        [XmlIgnore]
        public bool IsOnline {
            get {
                return CurrentConnection.IsOnline;
            }
        }

        [XmlIgnore]
        public bool IsBonjour {
            get {
                return KnownConnections.ContainsKey(typeof(BonjourConnectionInfo))
                    && KnownConnections[typeof(BonjourConnectionInfo)].IsOnline;
            }
        }

        [XmlIgnore]
        public bool IsMyPlex {
            get {
                return KnownConnections.ContainsKey(typeof(MyPlexConnectionInfo))
                    && KnownConnections[typeof(MyPlexConnectionInfo)].IsOnline;
            }
        }

        [XmlIgnore]
        public bool IsManual {
            get {
                return KnownConnections.ContainsKey(typeof(ManualConnectionInfo))
                  && KnownConnections[typeof(ManualConnectionInfo)].IsOnline;
            }
        }

        public void AddConnectionInfo(BaseConnectionInfo connectionInfo) {
            if (KnownConnections == null) {
                KnownConnections = new SerializableDictionary<Type, BaseConnectionInfo>();
            }
            if (KnownConnections.ContainsKey(connectionInfo.GetType())) {
                //Update connection info
                KnownConnections[connectionInfo.GetType()] = connectionInfo;
            } else {
                //Add new Connection info
                KnownConnections.Add(connectionInfo.GetType(), connectionInfo);
            }
        }

        public override string ToString() {
            return String.Format("{0} @ {1} [{2}]", FriendlyName, CurrentConnection.UriPlexBase, PlexVersion);
        }

        internal bool Authenticate(ref WebClient webClient) {
            return CurrentConnection.TryConnect(ref webClient);
        }

        #region IEquatable<PlexServer> Members

        public bool Equals(PlexServer other) {
            return MachineIdentifier.Equals(other.MachineIdentifier);
        }
        #endregion
    }
}