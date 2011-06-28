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
    public class MediaContainerDirectory {
        private string agentField;
        private string artField;

        private string bannerField;
        private string contentRatingField;

        private string durationField;
        private System.Collections.Generic.List<Genre> genreField;
        private string indexField;
        private string keyField;
        private string languageField;

        private string leafCountField;
        private System.Collections.Generic.List<MediaContainerDirectoryLocation> locationField;
        private string originallyAvailableAtField;
        private string parentTitleField;

        private string promptField;
        private string ratingField;
        private string ratingKeyField;

        private string refreshingField;

        private string scannerField;
        private string searchField;
        private string studioField;
        private string summaryField;
        private string thumbField;
        private string titleField;
        private string typeField;
        private string updatedAtField;
        private string viewedLeafCountField;
        private string yearField;

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Location", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<MediaContainerDirectoryLocation> Location {
            get { return locationField; }
            set { locationField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Genre")]
        public System.Collections.Generic.List<Genre> Genre {
            get { return genreField; }
            set { genreField = value; }
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
        public string studio {
            get { return studioField; }
            set { studioField = value; }
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
        public string contentRating {
            get { return contentRatingField; }
            set { contentRatingField = value; }
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
        public string rating {
            get { return ratingField; }
            set { ratingField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string year {
            get { return yearField; }
            set { yearField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string thumb {
            get { return thumbField; }
            set { thumbField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string art {
            get { return artField; }
            set { artField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string banner {
            get { return bannerField; }
            set { bannerField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string duration {
            get { return durationField; }
            set { durationField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string originallyAvailableAt {
            get { return originallyAvailableAtField; }
            set { originallyAvailableAtField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string leafCount {
            get { return leafCountField; }
            set { leafCountField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string viewedLeafCount {
            get { return viewedLeafCountField; }
            set { viewedLeafCountField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string updatedAt {
            get { return updatedAtField; }
            set { updatedAtField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string prompt {
            get { return promptField; }
            set { promptField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string search {
            get { return searchField; }
            set { searchField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string refreshing {
            get { return refreshingField; }
            set { refreshingField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string agent {
            get { return agentField; }
            set { agentField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string scanner {
            get { return scannerField; }
            set { scannerField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string language {
            get { return languageField; }
            set { languageField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string parentTitle {
            get { return parentTitleField; }
            set { parentTitleField = value; }
        }
    }
}