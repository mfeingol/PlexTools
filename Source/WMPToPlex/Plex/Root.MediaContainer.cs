// (c) 2019 Max Feingold

namespace WMPToPlex.Root
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class MediaContainer
    {

        private MediaContainerDirectory[] directoryField;

        private byte sizeField;

        private byte allowCameraUploadField;

        private byte allowChannelAccessField;

        private byte allowMediaDeletionField;

        private byte allowSharingField;

        private byte allowSyncField;

        private byte allowTunersField;

        private byte backgroundProcessingField;

        private byte certificateField;

        private byte companionProxyField;

        private string countryCodeField;

        private string diagnosticsField;

        private byte eventStreamField;

        private string friendlyNameField;

        private byte hubSearchField;

        private byte itemClustersField;

        private byte livetvField;

        private string machineIdentifierField;

        private byte mediaProvidersField;

        private byte multiuserField;

        private byte myPlexField;

        private string myPlexMappingStateField;

        private string myPlexSigninStateField;

        private byte myPlexSubscriptionField;

        private string myPlexUsernameField;

        private string ownerFeaturesField;

        private byte photoAutoTagField;

        private string platformField;

        private string platformVersionField;

        private byte pluginHostField;

        private byte pushNotificationsField;

        private byte readOnlyLibrariesField;

        private byte requestParametersInCookieField;

        private byte streamingBrainABRVersionField;

        private byte streamingBrainVersionField;

        private byte syncField;

        private byte transcoderActiveVideoSessionsField;

        private byte transcoderAudioField;

        private byte transcoderLyricsField;

        private byte transcoderPhotoField;

        private byte transcoderSubtitlesField;

        private byte transcoderVideoField;

        private string transcoderVideoBitratesField;

        private string transcoderVideoQualitiesField;

        private string transcoderVideoResolutionsField;

        private uint updatedAtField;

        private byte updaterField;

        private string versionField;

        private byte voiceSearchField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Directory")]
        public MediaContainerDirectory[] Directory
        {
            get
            {
                return this.directoryField;
            }
            set
            {
                this.directoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte allowCameraUpload
        {
            get
            {
                return this.allowCameraUploadField;
            }
            set
            {
                this.allowCameraUploadField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte allowChannelAccess
        {
            get
            {
                return this.allowChannelAccessField;
            }
            set
            {
                this.allowChannelAccessField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte allowMediaDeletion
        {
            get
            {
                return this.allowMediaDeletionField;
            }
            set
            {
                this.allowMediaDeletionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte allowSharing
        {
            get
            {
                return this.allowSharingField;
            }
            set
            {
                this.allowSharingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte allowSync
        {
            get
            {
                return this.allowSyncField;
            }
            set
            {
                this.allowSyncField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte allowTuners
        {
            get
            {
                return this.allowTunersField;
            }
            set
            {
                this.allowTunersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte backgroundProcessing
        {
            get
            {
                return this.backgroundProcessingField;
            }
            set
            {
                this.backgroundProcessingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte certificate
        {
            get
            {
                return this.certificateField;
            }
            set
            {
                this.certificateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte companionProxy
        {
            get
            {
                return this.companionProxyField;
            }
            set
            {
                this.companionProxyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string countryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string diagnostics
        {
            get
            {
                return this.diagnosticsField;
            }
            set
            {
                this.diagnosticsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte eventStream
        {
            get
            {
                return this.eventStreamField;
            }
            set
            {
                this.eventStreamField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string friendlyName
        {
            get
            {
                return this.friendlyNameField;
            }
            set
            {
                this.friendlyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte hubSearch
        {
            get
            {
                return this.hubSearchField;
            }
            set
            {
                this.hubSearchField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte itemClusters
        {
            get
            {
                return this.itemClustersField;
            }
            set
            {
                this.itemClustersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte livetv
        {
            get
            {
                return this.livetvField;
            }
            set
            {
                this.livetvField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string machineIdentifier
        {
            get
            {
                return this.machineIdentifierField;
            }
            set
            {
                this.machineIdentifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte mediaProviders
        {
            get
            {
                return this.mediaProvidersField;
            }
            set
            {
                this.mediaProvidersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte multiuser
        {
            get
            {
                return this.multiuserField;
            }
            set
            {
                this.multiuserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte myPlex
        {
            get
            {
                return this.myPlexField;
            }
            set
            {
                this.myPlexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string myPlexMappingState
        {
            get
            {
                return this.myPlexMappingStateField;
            }
            set
            {
                this.myPlexMappingStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string myPlexSigninState
        {
            get
            {
                return this.myPlexSigninStateField;
            }
            set
            {
                this.myPlexSigninStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte myPlexSubscription
        {
            get
            {
                return this.myPlexSubscriptionField;
            }
            set
            {
                this.myPlexSubscriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string myPlexUsername
        {
            get
            {
                return this.myPlexUsernameField;
            }
            set
            {
                this.myPlexUsernameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ownerFeatures
        {
            get
            {
                return this.ownerFeaturesField;
            }
            set
            {
                this.ownerFeaturesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte photoAutoTag
        {
            get
            {
                return this.photoAutoTagField;
            }
            set
            {
                this.photoAutoTagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string platform
        {
            get
            {
                return this.platformField;
            }
            set
            {
                this.platformField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string platformVersion
        {
            get
            {
                return this.platformVersionField;
            }
            set
            {
                this.platformVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte pluginHost
        {
            get
            {
                return this.pluginHostField;
            }
            set
            {
                this.pluginHostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte pushNotifications
        {
            get
            {
                return this.pushNotificationsField;
            }
            set
            {
                this.pushNotificationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte readOnlyLibraries
        {
            get
            {
                return this.readOnlyLibrariesField;
            }
            set
            {
                this.readOnlyLibrariesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte requestParametersInCookie
        {
            get
            {
                return this.requestParametersInCookieField;
            }
            set
            {
                this.requestParametersInCookieField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte streamingBrainABRVersion
        {
            get
            {
                return this.streamingBrainABRVersionField;
            }
            set
            {
                this.streamingBrainABRVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte streamingBrainVersion
        {
            get
            {
                return this.streamingBrainVersionField;
            }
            set
            {
                this.streamingBrainVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte sync
        {
            get
            {
                return this.syncField;
            }
            set
            {
                this.syncField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte transcoderActiveVideoSessions
        {
            get
            {
                return this.transcoderActiveVideoSessionsField;
            }
            set
            {
                this.transcoderActiveVideoSessionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte transcoderAudio
        {
            get
            {
                return this.transcoderAudioField;
            }
            set
            {
                this.transcoderAudioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte transcoderLyrics
        {
            get
            {
                return this.transcoderLyricsField;
            }
            set
            {
                this.transcoderLyricsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte transcoderPhoto
        {
            get
            {
                return this.transcoderPhotoField;
            }
            set
            {
                this.transcoderPhotoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte transcoderSubtitles
        {
            get
            {
                return this.transcoderSubtitlesField;
            }
            set
            {
                this.transcoderSubtitlesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte transcoderVideo
        {
            get
            {
                return this.transcoderVideoField;
            }
            set
            {
                this.transcoderVideoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string transcoderVideoBitrates
        {
            get
            {
                return this.transcoderVideoBitratesField;
            }
            set
            {
                this.transcoderVideoBitratesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string transcoderVideoQualities
        {
            get
            {
                return this.transcoderVideoQualitiesField;
            }
            set
            {
                this.transcoderVideoQualitiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string transcoderVideoResolutions
        {
            get
            {
                return this.transcoderVideoResolutionsField;
            }
            set
            {
                this.transcoderVideoResolutionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint updatedAt
        {
            get
            {
                return this.updatedAtField;
            }
            set
            {
                this.updatedAtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte updater
        {
            get
            {
                return this.updaterField;
            }
            set
            {
                this.updaterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte voiceSearch
        {
            get
            {
                return this.voiceSearchField;
            }
            set
            {
                this.voiceSearchField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerDirectory
    {

        private byte countField;

        private string keyField;

        private string titleField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
    }


}
