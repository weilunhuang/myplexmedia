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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using PlexMediaCenter.Plex;
using PlexMediaCenter.Plex.Connection;
using PlexMediaCenter.Plex.Data.Types;

namespace PlexMediaCenter.Util {
    public static class Transcoding {
        private const string PlexApiPublicKey = "KQMIY6GATPC63AIMC4R2";
        private const string PlexApiSharedSecret = "k3U6GLkZOoNIoSgjDshPErvqMIFdE0xMTx8kgsrhnC0=";

        public static Uri GetTrackPlaybackUrl(Uri plexUriPath, MediaContainerTrack track) {
            PlexServer server = PlexInterface.ServerManager.TryFindPlexServer(plexUriPath);
            return new Uri(server.UriPlexBase, track.Media[0].Part[0].key + GetPlexUserPass(server));
        }

        public static List<string> GetVideoSegmentedPlayList(Uri plexUriPath, MediaContainerVideo video, long offset,
                                                             int quality, bool is3G) {
            PlexServer server = PlexInterface.ServerManager.TryFindPlexServer(plexUriPath);
            List<string> tmpList = new List<string>();
            foreach (var media in video.Media) {
                foreach (var part in media.Part) {
                    tmpList.AddRange(Transcoding.GetM3U8PlaylistItems(server, part.key, offset, quality, is3G));
                }
            }
            return tmpList;
        }

        private static string GetPlexApiToken(string url, string time) {
            // the message to hash is url + an @ + the rounded time   
            string msg = url + "@" + time;
            byte[] privateKey = Convert.FromBase64String(PlexApiSharedSecret);

            // initialize a new HMACSHA256 class with the private key from Elan
            HMACSHA256 hmac = new HMACSHA256(privateKey);

            // compute the hash of the message. Note: .net is unicode double byte, so when we get the bytes
            // from the message we have to be sure to use UTF8 decoders.
            hmac.ComputeHash(Encoding.UTF8.GetBytes(msg));

            //our new super secret token is our new hash converted to a Base64 string
            return Convert.ToBase64String(hmac.Hash);
        }

        private static string GetUnixTime() {
            // unix time is the number of milliseconds from 1/1/1970 to now..          
            DateTime jan1 = new DateTime(1970, 1, 1, 0, 0, 0);
            double dTime = (DateTime.Now - jan1).TotalMilliseconds;
            // as per the Javascript example, round up the Unix time
            string time = Math.Round(dTime/1000).ToString();
            // the basic url WITH the part key is:
            return time;
        }

        private static IEnumerable<string> GetM3U8PlaylistItems(PlexServer plexServer, string partKey, long offset,
                                                                int quality, bool is3G) {
            List<string> playListItems;
            string playListRequest = plexServer.UriPlexBase.AbsoluteUri + "video/:/transcode/segmented/";
            using (WebClient plexClient = new WebClient()) {
                string response = plexClient.DownloadString(GetM3U8PlaylistUrl(plexServer, partKey, offset,
                                                                               quality, is3G));
                string session = response.Substring(response.IndexOf("session")).Replace("\n", "");
                playListRequest += session;
                plexClient.Headers[HttpRequestHeader.Cookie] = plexClient.ResponseHeaders[HttpResponseHeader.SetCookie];
                string playList = plexClient.DownloadString(playListRequest);
                playListItems = playList.Split(new[] {'\n'}).Where(item => item.EndsWith(".ts")).ToList();
            }
            return playListItems.Select(currentItem => playListRequest.Replace("index.m3u8", currentItem));
        }

        private static Uri GetM3U8PlaylistUrl(PlexServer plexServer, string partKey, long offset,
                                              int quality, bool is3G) {
            string transcodePath = "/video/:/transcode/segmented/start.m3u8?";
            transcodePath += "identifier=com.plexapp.plugins.library";
            transcodePath += "&offset=" + offset;
            transcodePath += "&qualitiy=" + quality;
            //transcodePath += "&minquality=0";
            //transcodePath += "&maxquality=1";
            transcodePath += "&3g=" + (is3G ? "1" : "0");
            transcodePath += "&url=" + Uri.EscapeDataString("http://localhost:32400" + partKey);
            transcodePath += GetPlexAuthParameters(plexServer, transcodePath);
            transcodePath += PlexCapabilitiesClient.GetClientCapabilities();
            //transcodePath += "&httpCookies=";
            //transcodePath += "&userAgent=";
            return new Uri(plexServer.UriPlexBase + transcodePath.Remove(0, 1));
        }

        private static string GetPlexAuthParameters(PlexServer plexServer, string url) {
            string time = GetUnixTime();
            string authParameters = string.Empty;
            authParameters += "&X-Plex-User=" + plexServer.UserName;
            authParameters += "&X-Plex-Pass=" + plexServer.UserPass;
            authParameters += "&X-Plex-Access-Key=" + PlexApiPublicKey;
            authParameters += "&X-Plex-Access-Time=" + time;
            authParameters += "&X-Plex-Access-Code=" + Uri.EscapeDataString(GetPlexApiToken(url, time));
            return authParameters;
        }

        private static string GetPlexUserPass(PlexServer plexServer) {
            return String.Format("?X-Plex-User={0}&X-Plex-Pass={1}", plexServer.UserName, plexServer.UserPass);
        }

        //public static Uri GetFlvStreamUrl(PlexServer plexServer, string partKey, long offset = 0, int quality = _defaultQuality, bool is3G = false) {
        //    //Request: GET /video/:/transcode/generic.flv?format=flv&videoCodec=libx264&vpre=video-embedded-h264&videoBitrate=5000&audioCodec=libfaac&apre=audio-embedded-aac&audioBitrate=128&size=640x480&fakeContentLength=2000000000&url=http%3A%2F%2F192%2E168%2E1%2E87%3A32400%2Fvideo%2F
        //    string transcodePath = "/video/:/transcode/generic.flv?";
        //    transcodePath += "format=flv";
        //    transcodePath += "&videoCodec=libx264&vpre=video-embedded-h264&videoBitrate=5000&audioCodec=libfaac&apre=audio-embedded-aac&audioBitrate=128&size=640x480&fakeContentLength=2000000000";
        //    transcodePath += "&url=" + Uri.EscapeDataString("http://localhost:32400" + partKey);
        //    transcodePath += GetPlexAuthParameters(plexServer, transcodePath);
        //    transcodePath += PlexCapabilitiesClient.GetClientCapabilities();
        //    //transcodePath += "&httpCookies=";
        //    //transcodePath += "&userAgent=";

        //    return new Uri(plexServer.UriPlexBase + transcodePath.Remove(0, 1));
        //}
    }
}