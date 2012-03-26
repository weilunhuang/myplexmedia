using System;
using System.Net;

namespace PlexMediaCenter.Plex.Connection {
    [Serializable]
    public class MyPlexConnectionInfo : BaseConnectionInfo {

        public string AuthToken { get; set; }

        public MyPlexConnectionInfo() {
        }

        public MyPlexConnectionInfo(string hostName, string hostAdress, int plexPort, string authToken)
            : base(hostName, hostAdress, plexPort) {
            AuthToken = authToken;
        }

        public override void AddAuthHeaders(ref WebClient webClient) {
            webClient.Headers["X-Plex-Token"] = AuthToken;
        }
    }
}
