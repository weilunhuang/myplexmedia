using System;
using System.Net;

namespace PlexMediaCenter.Plex.Connection {
    [Serializable]
    public class BonjourConnectionInfo : BaseConnectionInfo {

        public BonjourConnectionInfo() {
        }

        public BonjourConnectionInfo(string hostName, string hostAdress, int plexBonjourPort) 
        :base(hostName,hostAdress, plexBonjourPort){
        }

        public override void AddAuthHeaders(ref WebClient webClient) {

        }
    }
}
