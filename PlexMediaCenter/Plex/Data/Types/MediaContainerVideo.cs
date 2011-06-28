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
    public class MediaContainerVideo {
        private string addedAtField;
        private string artField;
        private string contentRatingField;
        private System.Collections.Generic.List<MediaContainerVideoDirector> directorField;
        private string durationField;

        private System.Collections.Generic.List<MediaContainerVideoField> fieldField;
        private System.Collections.Generic.List<Genre> genreField;
        private string grandparentKeyField;

        private string grandparentTitleField;

        private string guidField;
        private string indexField;
        private string keyField;
        private System.Collections.Generic.List<Media> mediaField;
        private string originallyAvailableAtField;
        private string parentIndexField;
        private string ratingField;
        private string ratingKeyField;

        private string studioField;

        private string summaryField;

        private string taglineField;

        private string thumbField;
        private string titleField;
        private string typeField;

        private string updatedAtField;

        private string viewOffsetField;
        private string yearField;

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Media")]
        public System.Collections.Generic.List<Media> Media {
            get { return mediaField; }
            set { mediaField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Genre")]
        public System.Collections.Generic.List<Genre> Genre {
            get { return genreField; }
            set { genreField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Director", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<MediaContainerVideoDirector> Director {
            get { return directorField; }
            set { directorField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Field", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<MediaContainerVideoField> Field {
            get { return fieldField; }
            set { fieldField = value; }
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
        public string guid {
            get { return guidField; }
            set { guidField = value; }
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
        public string tagline {
            get { return taglineField; }
            set { taglineField = value; }
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
        public string addedAt {
            get { return addedAtField; }
            set { addedAtField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string updatedAt {
            get { return updatedAtField; }
            set { updatedAtField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string grandparentKey {
            get { return grandparentKeyField; }
            set { grandparentKeyField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string grandparentTitle {
            get { return grandparentTitleField; }
            set { grandparentTitleField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string index {
            get { return indexField; }
            set { indexField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string parentIndex {
            get { return parentIndexField; }
            set { parentIndexField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string viewOffset {
            get { return viewOffsetField; }
            set { viewOffsetField = value; }
        }
    }
}