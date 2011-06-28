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

using System.Xml.Serialization;

namespace PlexMediaCenter.Plex.Data.Types {
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute]
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class MediaContainer {
        private string artField;
        private string bannerField;
        private System.Collections.Generic.List<MediaContainerDirectory> directoryField;
        private string friendlyNameField;

        private string grandparentTitleField;
        private string identifierField;
        private string keyField;
        private string machineIdentifierField;

        private string mediaTagPrefixField;

        private string mediaTagVersionField;

        private string nocacheField;

        private string parentIndexField;

        private string parentTitleField;

        private string parentYearField;
        private string sizeField;

        private string thumbField;

        private string title1Field;

        private string title2Field;
        private System.Collections.Generic.List<MediaContainerTrack> trackField;

        private string transcoderActiveVideoSessionsField;

        private string transcoderVideoBitratesField;

        private string transcoderVideoQualitiesField;

        private string transcoderVideoResolutionsField;

        private string versionField;
        private System.Collections.Generic.List<MediaContainerVideo> videoField;
        private string viewGroupField;

        private string viewModeField;

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Directory", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<MediaContainerDirectory> Directory {
            get { return directoryField; }
            set { directoryField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Video", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<MediaContainerVideo> Video {
            get { return videoField; }
            set { videoField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Track", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<MediaContainerTrack> Track {
            get { return trackField; }
            set { trackField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string size {
            get { return sizeField; }
            set { sizeField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string grandparentTitle {
            get { return grandparentTitleField; }
            set { grandparentTitleField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string mediaTagPrefix {
            get { return mediaTagPrefixField; }
            set { mediaTagPrefixField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string mediaTagVersion {
            get { return mediaTagVersionField; }
            set { mediaTagVersionField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string nocache {
            get { return nocacheField; }
            set { nocacheField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string banner {
            get { return bannerField; }
            set { bannerField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string parentIndex {
            get { return parentIndexField; }
            set { parentIndexField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string parentTitle {
            get { return parentTitleField; }
            set { parentTitleField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string parentYear {
            get { return parentYearField; }
            set { parentYearField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string thumb {
            get { return thumbField; }
            set { thumbField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string viewGroup {
            get { return viewGroupField; }
            set { viewGroupField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string viewMode {
            get { return viewModeField; }
            set { viewModeField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string key {
            get { return keyField; }
            set { keyField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string art {
            get { return artField; }
            set { artField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string title1 {
            get { return title1Field; }
            set { title1Field = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string title2 {
            get { return title2Field; }
            set { title2Field = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string identifier {
            get { return identifierField; }
            set { identifierField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string friendlyName {
            get { return friendlyNameField; }
            set { friendlyNameField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string machineIdentifier {
            get { return machineIdentifierField; }
            set { machineIdentifierField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string transcoderActiveVideoSessions {
            get { return transcoderActiveVideoSessionsField; }
            set { transcoderActiveVideoSessionsField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string transcoderVideoBitrates {
            get { return transcoderVideoBitratesField; }
            set { transcoderVideoBitratesField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string transcoderVideoQualities {
            get { return transcoderVideoQualitiesField; }
            set { transcoderVideoQualitiesField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string transcoderVideoResolutions {
            get { return transcoderVideoResolutionsField; }
            set { transcoderVideoResolutionsField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string version {
            get { return versionField; }
            set { versionField = value; }
        }

        [XmlIgnore]
        public System.Uri UriSource { get; set; }
    }
}