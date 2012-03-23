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

namespace PlexMediaCenter.Plex.Connection {
    public static class PlexCapabilitiesClient {
        static PlexCapabilitiesClient() {
            Protocols = new List<string>
                            {
                                "http-live-streaming",
                                "http-mp4-streaming",
                                "http-streaming-video",
                                "http-streaming-video-240p",
                                "http-streaming-video-320p",
                                "http-streaming-video-480p",
                                "http-streaming-video-720p",
                                "http-streaming-video-1080p",
                                "http-mp4-video",
                                "http-mp4-video-240p",
                                "http-mp4-video-320p",
                                "http-mp4-video-480p",
                                "http-mp4-video-720p",
                                "http-mp4-video-1080p",                                
                            };

            VideoDecoders = new List<string>
                                {
                                    "h264{profile:high&resolution:1080&level:51}",
                                    "h264{profile:high&resolution:720&level:51}"
                                };

            AudioDecoders = new List<string>
                                {
                                    "mp3"                                    
                                };
        }

        private static List<string> Protocols { get; set; }
        private static List<string> VideoDecoders { get; set; }
        private static List<string> AudioDecoders { get; set; }

        public static string GetClientCapabilities() {
            return "&X-Plex-Client-Capabilities=" +
                   Uri.EscapeDataString(String.Format("protocols={0};videoDecoders={1};audioDecoders={2};",
                                                      String.Join(",", Protocols.ToArray()),
                                                      String.Join(",", VideoDecoders.ToArray()),
                                                      String.Join(",", AudioDecoders.ToArray())));
        }
    }
}