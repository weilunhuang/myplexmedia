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

        public MyPlexUser MyPlexAccount { get; private set; }
        public List<PlexServer> MyPlexServerList { get; private set; }

        public MyPlex(NetworkCredential myPlexCredentials) {
            MyPlexCredentials = myPlexCredentials;
        }

        public bool Authenticate(ref WebClient webClient) {
            webClient.Headers["X-Plex-Platform"] = string.Empty;
            webClient.Headers["X-Plex-Platform-Version"] = string.Empty;
            webClient.Headers["X-Plex-Provides"] = string.Empty;
            webClient.Headers["X-Plex-Product"] = string.Empty;
            webClient.Headers["X-Plex-Device"] = string.Empty;
            webClient.Headers["X-Plex-Client-Identifier"] = new Guid().ToString();
            try {
                webClient.Credentials = MyPlexCredentials;
                byte[] byteResult = webClient.UploadData(LoginUrl, "POST", new byte[0]);
                MyPlexAccount = Serialization.DeSerializeXML<MyPlexUser>(Encoding.ASCII.GetString(byteResult));
                webClient.Headers["X-Plex-Token"] = MyPlexAccount.authenticationtoken;
                string serversRes = webClient.DownloadString(ServersUrl);
                List<MediaContainerServer> servers= Serialization.DeSerializeXML<MediaContainer>(serversRes).Server;
                MyPlexServerList = servers.ConvertAll<PlexServer>(mcs => new PlexServer(mcs.name , mcs.host, int.Parse(mcs.port), MyPlexAccount));
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
