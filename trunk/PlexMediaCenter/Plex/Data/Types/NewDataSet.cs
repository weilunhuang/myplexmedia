namespace PlexMediaCenter.Plex.Data.Types
{
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class NewDataSet
    {
        
        private System.Collections.Generic.List<object> itemsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Genre", typeof(Genre))]
        [System.Xml.Serialization.XmlElementAttribute("Media", typeof(Media))]
        [System.Xml.Serialization.XmlElementAttribute("MediaContainer", typeof(MediaContainer))]
        public System.Collections.Generic.List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }
}
