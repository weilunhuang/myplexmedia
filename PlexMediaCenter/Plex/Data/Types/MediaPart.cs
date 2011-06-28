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
    public class MediaPart {
        private string durationField;

        private string fileField;
        private string idField;

        private string keyField;

        private string sizeField;
        private System.Collections.Generic.List<MediaPartStream> streamField;

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Stream", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<MediaPartStream> Stream {
            get { return streamField; }
            set { streamField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string id {
            get { return idField; }
            set { idField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string key {
            get { return keyField; }
            set { keyField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string duration {
            get { return durationField; }
            set { durationField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string file {
            get { return fileField; }
            set { fileField = value; }
        }

        /// <remarks />
        [System.Xml.Serialization.XmlAttributeAttribute]
        public string size {
            get { return sizeField; }
            set { sizeField = value; }
        }
    }
}