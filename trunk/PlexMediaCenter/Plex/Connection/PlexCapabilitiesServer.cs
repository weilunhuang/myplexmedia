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

using System.Collections.Generic;
using System.Linq;
using PlexMediaCenter.Plex.Data.Types;

namespace PlexMediaCenter.Plex.Connection {
    public class PlexCapabilitiesServer {
        public PlexCapabilitiesServer() {
        }

        public PlexCapabilitiesServer(MediaContainer serverBaseUriResponse) {
            FriendlyName = serverBaseUriResponse.friendlyName;
            PMSVersion = serverBaseUriResponse.version;
            MachineIdentifier = serverBaseUriResponse.machineIdentifier;
            TranscoderVideoBitrates = serverBaseUriResponse.transcoderVideoBitrates.Split(',').ToList();
            TranscoderVideoQualities = serverBaseUriResponse.transcoderVideoQualities.Split(',').ToList();
            TranscoderVideoResolutions = serverBaseUriResponse.transcoderVideoResolutions.Split(',').ToList();
        }

        public string MachineIdentifier { get; set; }
        public string FriendlyName { get; set; }
        public string PMSVersion { get; set; }
        public List<string> TranscoderVideoBitrates { get; set; }
        public int SelectedBitrateIndex { get; set; }
        public List<string> TranscoderVideoQualities { get; set; }
        public int SelectedQualityIndex { get; set; }
        public List<string> TranscoderVideoResolutions { get; set; }
        public int SelectedVideoResolutionIndex { get; set; }
    }
}