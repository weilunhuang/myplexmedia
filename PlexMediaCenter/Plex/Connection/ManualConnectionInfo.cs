using System;
using System.Net;
using PlexMediaCenter.Util;

namespace PlexMediaCenter.Plex.Connection {
    [Serializable]
    public class ManualConnectionInfo : BaseConnectionInfo {

        public string UserName { get; set; }
        public string UserPass { get; set; }

        public ManualConnectionInfo() {
        }

        public ManualConnectionInfo(string hostName, string hostAdress, int plexPort, string userName, string userPass) 
            : base(hostName, hostAdress, plexPort){
            UserName = userName;
            UserPass = EncryptPassword(userName, userPass);
        }

        public override void AddAuthHeaders(ref WebClient webClient) {
            webClient.Headers["X-Plex-User"] = UserName;
            webClient.Headers["X-Plex-Pass"] = UserPass;
        }

        private static string EncryptPassword(string userName, string userPass) {
            return Encryption.GetSHA1Hash(userName.ToLower() + Encryption.GetSHA1Hash(userPass));
        }
    }
}
