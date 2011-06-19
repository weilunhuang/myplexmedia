using System.Xml.Serialization;
namespace PlexMediaCenter.Plex.Data.Types {


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class MediaContainer {

        private System.Collections.Generic.List<MediaContainerDirectory> directoryField;

        private System.Collections.Generic.List<MediaContainerVideo> videoField;

        private System.Collections.Generic.List<MediaContainerTrack> trackField;

        private string sizeField;

        private string grandparentTitleField;

        private string mediaTagPrefixField;

        private string mediaTagVersionField;

        private string nocacheField;

        private string bannerField;

        private string parentIndexField;

        private string parentTitleField;

        private string parentYearField;

        private string thumbField;

        private string viewGroupField;

        private string viewModeField;

        private string keyField;

        private string artField;

        private string title1Field;

        private string title2Field;

        private string identifierField;

        private string friendlyNameField;

        private string machineIdentifierField;

        private string transcoderActiveVideoSessionsField;

        private string transcoderVideoBitratesField;

        private string transcoderVideoQualitiesField;

        private string transcoderVideoResolutionsField;

        private string versionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Directory", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<MediaContainerDirectory> Directory {
            get {
                return this.directoryField;
            }
            set {
                this.directoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Video", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<MediaContainerVideo> Video {
            get {
                return this.videoField;
            }
            set {
                this.videoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Track", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<MediaContainerTrack> Track {
            get {
                return this.trackField;
            }
            set {
                this.trackField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string size {
            get {
                return this.sizeField;
            }
            set {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string grandparentTitle {
            get {
                return this.grandparentTitleField;
            }
            set {
                this.grandparentTitleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string mediaTagPrefix {
            get {
                return this.mediaTagPrefixField;
            }
            set {
                this.mediaTagPrefixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string mediaTagVersion {
            get {
                return this.mediaTagVersionField;
            }
            set {
                this.mediaTagVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nocache {
            get {
                return this.nocacheField;
            }
            set {
                this.nocacheField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string banner {
            get {
                return this.bannerField;
            }
            set {
                this.bannerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parentIndex {
            get {
                return this.parentIndexField;
            }
            set {
                this.parentIndexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parentTitle {
            get {
                return this.parentTitleField;
            }
            set {
                this.parentTitleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parentYear {
            get {
                return this.parentYearField;
            }
            set {
                this.parentYearField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string thumb {
            get {
                return this.thumbField;
            }
            set {
                this.thumbField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string viewGroup {
            get {
                return this.viewGroupField;
            }
            set {
                this.viewGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string viewMode {
            get {
                return this.viewModeField;
            }
            set {
                this.viewModeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string art {
            get {
                return this.artField;
            }
            set {
                this.artField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title1 {
            get {
                return this.title1Field;
            }
            set {
                this.title1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title2 {
            get {
                return this.title2Field;
            }
            set {
                this.title2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string identifier {
            get {
                return this.identifierField;
            }
            set {
                this.identifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string friendlyName {
            get {
                return this.friendlyNameField;
            }
            set {
                this.friendlyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string machineIdentifier {
            get {
                return this.machineIdentifierField;
            }
            set {
                this.machineIdentifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string transcoderActiveVideoSessions {
            get {
                return this.transcoderActiveVideoSessionsField;
            }
            set {
                this.transcoderActiveVideoSessionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string transcoderVideoBitrates {
            get {
                return this.transcoderVideoBitratesField;
            }
            set {
                this.transcoderVideoBitratesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string transcoderVideoQualities {
            get {
                return this.transcoderVideoQualitiesField;
            }
            set {
                this.transcoderVideoQualitiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string transcoderVideoResolutions {
            get {
                return this.transcoderVideoResolutionsField;
            }
            set {
                this.transcoderVideoResolutionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }

        [XmlIgnore]
        public System.Uri UriSource { get; set; }
    }
}
