using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlexMediaCenter.Plex;
using System.IO;
using PlexMediaCenter.Plex.Connection;

namespace PlexMediaCenter.Util {

    public class ServerManager {       

        public static event OnPlexServersChangedEventHandler OnPlexServersChanged;
        public delegate void OnPlexServersChangedEventHandler(List<PlexServer> updatedServerList);

        private string ServerXmlFile { get; set; }
       
        public  ServerManager(string serverXmlFile) {
            if(string.IsNullOrEmpty(serverXmlFile)){
                throw new Exception();
            }
            ServerXmlFile = serverXmlFile;
            PlexServers = LoadPlexServers();           
            BonjourDiscovery.OnBonjourServer += new BonjourDiscovery.OnBonjourServerEventHandler(BonjourDiscovery_OnBonjourServer);            
        }

        ~ServerManager() {
            SavePlexServers(PlexServers);
        }       

        public List<PlexServer> PlexServers { get; private set; }

        private PlexServer _plexServerCurrent;
        public PlexServer PlexServerCurrent {
            get {
                return _plexServerCurrent;
            }
            private set {
                _plexServerCurrent = value;
                if (!PlexServers.Contains(value)) {
                    PlexServers.Insert(0, value);                    
                }
                SavePlexServers(PlexServers);               
            }
        }

        private List<PlexServer> LoadPlexServers() {
            List<PlexServer> plexServers = new List<PlexServer>();
            if (File.Exists(ServerXmlFile)) {
                try {
                    plexServers = Serialization.DeSerialize<List<PlexServer>>(ServerXmlFile);
                } catch (Exception e) {
                    //throw e;
                }
            }
            return plexServers;
        }

        private void SavePlexServers(List<PlexServer> plexServers) {
            
            if (plexServers == null) {
                return;
            }
            try {
                Serialization.Serialize(ServerXmlFile, plexServers);
            } catch (Exception e){
                throw e;
            }
        }



        public void SetPlexServer(PlexServer server) {
            PlexServerCurrent = server;
        }

        void BonjourDiscovery_OnBonjourServer(PlexServer bonjourDiscoveredServer) {
            if (PlexServers.Contains<PlexServer>(bonjourDiscoveredServer)) {
                PlexServers.Find(x=> x.Equals(bonjourDiscoveredServer)).IsBonjour = true;
            }else{
                PlexServers.Add(bonjourDiscoveredServer);
                OnPlexServersChanged(PlexServers);
            }
        }

        public void RefrehBonjourServers() {            
            if (PlexServers.Count > 0) {
                OnPlexServersChanged(PlexServers);
            }
            BonjourDiscovery.RefreshBonjourDiscovery();
        }               


        internal bool Authenticate(ref System.Net.WebClient _webClient, PlexServer plexServer) {
            if (plexServer.Authenticate(ref _webClient)) {
                PlexServerCurrent = plexServer;
                return true;
            } else {
                return false;
            }
        }
    }
}
