using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using PlexMediaCenter.Util;
using PlexMediaCenter.Plex.Data.Types;

namespace PlexMediaCenter.Plex.Connection {
    public class MyPlex {

        private const string LoginUrl = "https://my.plexapp.com/users/sign_in.xml";
        private const string ServersUrl = "https://my.plexapp.com/pms/servers";

        private NetworkCredential MyPlexCredentials { get; set; }

        public user MyPlexAccount { get; private set; }
        public List<MyPlexConnectionInfo> MyPlexServers { get; private set; }

        public MyPlex(NetworkCredential myPlexCredentials) {
            MyPlexCredentials = myPlexCredentials;
        }

        public bool Authenticate(ref WebClient webClient) {
            webClient.Headers["X-Plex-Platform"] = "Microsoft Windows";
            webClient.Headers["X-Plex-Platform-Version"] = System.Environment.OSVersion.VersionString;
            webClient.Headers["X-Plex-Provides"] = "player";
            webClient.Headers["X-Plex-Product"] = "MyPlexMedia (MediaPortal Plugin)";
            webClient.Headers["X-Plex-Device"] = "HTPC (MediaPortal)";
            webClient.Headers["X-Plex-Client-Identifier"] = new Guid().ToString();
            try {
                webClient.Credentials = MyPlexCredentials;
                byte[] byteResult = webClient.UploadData(LoginUrl, "POST", new byte[0]);
                string s = Encoding.ASCII.GetString(byteResult);
                MyPlexAccount = Serialization.DeSerializeXML<user>(Encoding.ASCII.GetString(byteResult));
                webClient.Headers["X-Plex-Token"] = MyPlexAccount.authenticationtoken;
                string serversRes = webClient.DownloadString(ServersUrl);
                List<MediaContainerServer> servers = Serialization.DeSerializeXML<MediaContainer>(serversRes).Server;
                MyPlexServers = servers.ConvertAll<MyPlexConnectionInfo>(mcs => new MyPlexConnectionInfo(mcs.machineIdentifier, mcs.name, mcs.host, int.Parse(mcs.port), MyPlexAccount.authenticationtoken));
                return
                    MyPlexAccount != null
                    && !String.IsNullOrEmpty(MyPlexAccount.authenticationtoken)
                    && !String.IsNullOrEmpty(serversRes);
            } catch {
                return false;
            }
        }
    }
}
