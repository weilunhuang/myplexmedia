using System;
using System.Net;

namespace PlexMediaCenter.Plex.Connection {
    public class BonjourConnectionInfo : BaseConnectionInfo {

        public BonjourConnectionInfo() {
        }

        public BonjourConnectionInfo(string hostName, string hostAdress, int plexBonjourPort) 
        :base(hostName,hostAdress, plexBonjourPort){
        }

        internal override void AddAuthHeaders(WebClient webClient) {

        }

        internal override string GetAuthUrlParameters() {
            return string.Empty;
        }
    }
}
