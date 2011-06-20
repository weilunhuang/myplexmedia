﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlexMediaCenter.Util;
using System.Xml.Serialization;
using System.Net;
using PlexMediaCenter.Plex.Data.Types;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace PlexMediaCenter.Plex.Connection {
    [Serializable]
    public class PlexServer : IEquatable<PlexServer> {

        public String FriendlyName { get { return ServerCapabilities != null ? ServerCapabilities.FriendlyName : String.Empty; } }
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

        const int PlexPort = 32400;

        public PlexServer() { }

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

        public void EncryptPassword(string userName, string userPass) {
            UserPass = Encryption.GetSHA1Hash(userName.ToLower() + Encryption.GetSHA1Hash(userPass));
        }

        public override string ToString() {
            return String.Format("{0}@{1}", UserName, UriPlexBase.Host);
        }

        [XmlIgnore]
        public Uri UriPlexSections {
            get {
                return new Uri(UriPlexBase, "/library/sections/");
            }
        }

        [XmlIgnore]
        public Uri UriPlexBase { get { return new UriBuilder("http", HostAdress, PlexPort, "").Uri; } }

        public bool Equals(PlexServer other) {
            return HostAdress.Equals(other.HostAdress);
        }

        public void AddAuthHeaders(ref WebClient webClient) {
            webClient.Headers["X-Plex-User"] = this.UserName;
            webClient.Headers["X-Plex-Pass"] = this.UserPass;
        }

        internal bool Authenticate(ref WebClient webClient) {
            if (CheckSocketConnection()) {
                webClient.Headers["X-Plex-User"] = this.UserName;
                webClient.Headers["X-Plex-Pass"] = this.UserPass;
                try {
                    string serverXmlResponse = webClient.DownloadString(this.UriPlexBase);
                    ServerCapabilities = GetServerCapabilities(Serialization.DeSerializeXML<MediaContainer>(serverXmlResponse));
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
                    return result.AsyncWaitHandle.WaitOne(3000, true);                   
                } finally {                    
                    socket.Close();
                }
            }
        }

        private PlexCapabilitiesServer GetServerCapabilities(MediaContainer serverResponse) {
            if (serverResponse == null) {
                return null;
            }
            return new PlexCapabilitiesServer(serverResponse);
        }
    }
}
