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
    public class MediaContainerTrack {
        private string addedAtField;
        private string durationField;
        private string guidField;
        private string indexField;
        private string keyField;
        private System.Collections.Generic.List<Media> mediaField;

        private string ratingKeyField;

        private string summaryField;
        private string titleField;
        private string typeField;

        private string updatedAtField;

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Media")]
        public System.Collections.Generic.List<Media> Media {
            get { return mediaField; }
            set { mediaField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string ratingKey {
            get { return ratingKeyField; }
            set { ratingKeyField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string key {
            get { return keyField; }
            set { keyField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string type {
            get { return typeField; }
            set { typeField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string title {
            get { return titleField; }
            set { titleField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string summary {
            get { return summaryField; }
            set { summaryField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string index {
            get { return indexField; }
            set { indexField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string duration {
            get { return durationField; }
            set { durationField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string updatedAt {
            get { return updatedAtField; }
            set { updatedAtField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string guid {
            get { return guidField; }
            set { guidField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string addedAt {
            get { return addedAtField; }
            set { addedAtField = value; }
        }
    }
}