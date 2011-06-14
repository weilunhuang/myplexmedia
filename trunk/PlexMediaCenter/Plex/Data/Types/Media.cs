namespace PlexMediaCenter.Plex.Data.Types
{
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Media
    {
        
        private System.Collections.Generic.List<MediaPart> partField;
        
        private string idField;
        
        private string durationField;
        
        private string bitrateField;
        
        private string aspectRatioField;
        
        private string audioChannelsField;
        
        private string audioCodecField;
        
        private string videoCodecField;
        
        private string videoResolutionField;
        
        private string containerField;
        
        private string videoFrameRateField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Part", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public System.Collections.Generic.List<MediaPart> Part
        {
            get
            {
                return this.partField;
            }
            set
            {
                this.partField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bitrate
        {
            get
            {
                return this.bitrateField;
            }
            set
            {
                this.bitrateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string aspectRatio
        {
            get
            {
                return this.aspectRatioField;
            }
            set
            {
                this.aspectRatioField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string audioChannels
        {
            get
            {
                return this.audioChannelsField;
            }
            set
            {
                this.audioChannelsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string audioCodec
        {
            get
            {
                return this.audioCodecField;
            }
            set
            {
                this.audioCodecField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string videoCodec
        {
            get
            {
                return this.videoCodecField;
            }
            set
            {
                this.videoCodecField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string videoResolution
        {
            get
            {
                return this.videoResolutionField;
            }
            set
            {
                this.videoResolutionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string container
        {
            get
            {
                return this.containerField;
            }
            set
            {
                this.containerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string videoFrameRate
        {
            get
            {
                return this.videoFrameRateField;
            }
            set
            {
                this.videoFrameRateField = value;
            }
        }
    }
}
