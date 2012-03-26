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
using PlexMediaCenter.Plex.Connection;
using ZeroconfService;

namespace PlexMediaCenter.Util {
    public static class BonjourDiscovery {
        #region Delegates

        public delegate void OnBonjourEventHandler(BonjourConnectionInfo bojourDiscoveredServer);

        #endregion

        static BonjourDiscovery() {
            BonjourBrowser = new NetServiceBrowser();
            BonjourBrowser.DidFindDomain += _browser_DidFindDomain;
            BonjourBrowser.DidFindService += _browser_DidFindService;
        }

        private static NetServiceBrowser BonjourBrowser { get; set; }

        public static event OnBonjourEventHandler OnBonjourConnection;

        private static void _browser_DidFindService(NetServiceBrowser browser, NetService service, bool moreComing) {
            try {
                service.DidResolveService += service_DidResolveService;
                service.ResolveWithTimeout(60);
            } catch (Exception e) {
                throw e;
            }
        }

        private static void service_DidResolveService(NetService service) {
            try {                
                OnBonjourConnection(new BonjourConnectionInfo(service.HostName, ((IPEndPoint)service.Addresses[0]).Address.ToString(), service.Port));
            } catch (Exception e) {
                throw e;
            }
        }

        private static void _browser_DidFindDomain(NetServiceBrowser browser, string domainName, bool moreComing) {
            try {
                BonjourBrowser.SearchForService("_plexmediasvr._tcp", domainName);
            } catch (Exception e) {
                throw e;
            }
        }

        public static void StartBonjourDiscovery() {
            try {
                BonjourBrowser.SearchForBrowseableDomains();
            } catch {
                //Debug ("Couldn't locate servers via Bonjour.");
            }
        }

        public static void RefreshBonjourDiscovery() {
            try {
                BonjourBrowser.Stop();
                BonjourBrowser.SearchForBrowseableDomains();
            } catch {
                //Debug ("Couldn't locate servers via Bonjour.");
            }
        }
    }
}