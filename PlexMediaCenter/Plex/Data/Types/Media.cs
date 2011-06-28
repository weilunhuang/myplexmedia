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

namespace PlexMediaCenter.Plex.Data.Types {
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute]
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class Media {
        private string aspectRatioField;

        private string audioChannelsField;

        private string audioCodecField;
        private string bitrateField;

        private string containerField;
        private string durationField;
        private string idField;
        private System.Collections.Generic.List<MediaPart> partField;
        private string videoCodecField;

        private string videoFrameRateField;
        private string videoResolutionField;

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Part", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<MediaPart> Part {
            get { return partField; }
            set { partField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string id {
            get { return idField; }
            set { idField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string duration {
            get { return durationField; }
            set { durationField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string bitrate {
            get { return bitrateField; }
            set { bitrateField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string aspectRatio {
            get { return aspectRatioField; }
            set { aspectRatioField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string audioChannels {
            get { return audioChannelsField; }
            set { audioChannelsField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string audioCodec {
            get { return audioCodecField; }
            set { audioCodecField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string videoCodec {
            get { return videoCodecField; }
            set { videoCodecField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string videoResolution {
            get { return videoResolutionField; }
            set { videoResolutionField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string container {
            get { return containerField; }
            set { containerField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string videoFrameRate {
            get { return videoFrameRateField; }
            set { videoFrameRateField = value; }
        }
    }
}