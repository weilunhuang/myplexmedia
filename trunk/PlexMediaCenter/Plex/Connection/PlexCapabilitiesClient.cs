﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlexMediaCenter.Plex.Connection {

    public static class PlexCapabilitiesClient {

        private static List<string> Protocols { get; set; }
        private static List<string> VideoDecoders { get; set; }
        private static List<string> AudioDecoders { get; set; }

        static PlexCapabilitiesClient() {
            Protocols = new List<string>(){
                "http-live-streaming",
                "http-mp4-streaming",
                "http-streaming-video",
                "http-streaming-video-720p",
                "http-streaming-video-1080p",
                "http-mp4-video",
                "http-mp4-video-720p",
                "http-mp4-video-1080p"
            };

            VideoDecoders = new List<string>() { 
                "h264{profile:high&resolution:1080&level:51}",
            };

            AudioDecoders = new List<string>(){
                "mp3",
                "aac"
            };
        }

        public static string GetClientCapabilities() {
            return "&X-Plex-Client-Capabilities=" + Uri.EscapeDataString(String.Format("protocols={0};videoDecoders={1};audioDecoders={2};", String.Join(",", Protocols.ToArray()), String.Join(",", VideoDecoders.ToArray()), String.Join(",", AudioDecoders.ToArray())));
        }

        /* http-live-streaming,
         * http-mp4-streaming,
         * http-streaming-video,
         * http-streaming-video-720p,
         * * http-streaming-video-1080p,
         * http-mp4-video,
         * http-mp4-video-720p,
         * http-mp4-video-1080p
         * ;videoDecoders=h264{profile:high&resolution:1080&level:51};
         * audioDecoders=mp3,aac         
         */
    }
}
