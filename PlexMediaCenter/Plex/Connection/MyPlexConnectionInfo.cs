using System;
using System.Net;

namespace PlexMediaCenter.Plex.Connection {
    public class MyPlexConnectionInfo : BaseConnectionInfo {

        public string AuthToken { get; set; }

        public MyPlexConnectionInfo() {
        }

        public MyPlexConnectionInfo(string machineIdentifier, string hostName, string hostAdress, int plexPort, string authToken)
            : base(hostName, hostAdress, plexPort) {
            MachineIdentifier = machineIdentifier;
            AuthToken = authToken;
        }

        internal override void AddAuthHeaders(ref WebClient webClient) {
            webClient.Headers["X-Plex-Token"] = AuthToken;
        }

        internal override string GetAuthUrlParameters() {
            return String.Format("&X-Plex-Token={0}", AuthToken);
        }
    }
}
