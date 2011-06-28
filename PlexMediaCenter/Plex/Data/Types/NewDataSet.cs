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
    public class NewDataSet {
        private System.Collections.Generic.List<object> itemsField;

        /// <remarks />
        [System.Xml.Serialization.XmlElementAttribute("Genre", typeof (Genre))]
        [System.Xml.Serialization.XmlElementAttribute("Media", typeof (Media))]
        [System.Xml.Serialization.XmlElementAttribute("MediaContainer", typeof (MediaContainer))]
        public System.Collections.Generic.List<object> Items {
            get { return itemsField; }
            set { itemsField = value; }
        }
    }
}