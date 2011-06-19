using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlexMediaCenter.Plex.Data.Types;

namespace PlexMediaCenter.Plex.Connection {
    public class PlexCapabilitiesServer {

        public string FriendlyName { get; set; }
        public string PMSVersion { get; set; }
        public List<string> TranscoderVideoBitrates { get; set; }
        public int SelectedBitrateIndex { get; set; }
        public List<string> TranscoderVideoQualities { get; set; }
        public int SelectedQualityIndex { get; set; }
        public List<string> TranscoderVideoResolutions { get; set; }
        public int SelectedVideoResolutionIndex { get; set; }

        public PlexCapabilitiesServer() {

        }
        public PlexCapabilitiesServer(MediaContainer serverBaseUriResponse) {
            FriendlyName = serverBaseUriResponse.friendlyName;
            PMSVersion = serverBaseUriResponse.version;
            TranscoderVideoBitrates = serverBaseUriResponse.transcoderVideoBitrates.Split(',').ToList();
            TranscoderVideoQualities = serverBaseUriResponse.transcoderVideoQualities.Split(',').ToList();
            TranscoderVideoResolutions = serverBaseUriResponse.transcoderVideoResolutions.Split(',').ToList();
        }

    }
}
