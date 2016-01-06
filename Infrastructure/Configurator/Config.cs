﻿namespace ProcessingTools.Configurator
{
    using System.Runtime.Serialization;
    using System.Xml.Xsl;

    public partial class Config
    {
        private SchemaType articleSchemaType;
        private bool articleSchemaTypeStyleIsLockedForModification;

        private bool tagWholeDocument;
        private bool tagWholeDocumentIsLockedForModification;

        public Config()
        {
            this.articleSchemaType = SchemaType.System;
            this.articleSchemaTypeStyleIsLockedForModification = false;

            this.tagWholeDocument = false;
            this.tagWholeDocumentIsLockedForModification = false;
        }

        public SchemaType ArticleSchemaType
        {
            get
            {
                this.articleSchemaTypeStyleIsLockedForModification = true;
                return this.articleSchemaType;
            }

            set
            {
                if (!this.articleSchemaTypeStyleIsLockedForModification)
                {
                    this.articleSchemaType = value;
                }

                this.articleSchemaTypeStyleIsLockedForModification = true;
            }
        }

        public string EnvoResponseOutputXmlFileName { get; set; }

        public string GnrOutputFileName { get; set; }

        public bool TagWholeDocument
        {
            get
            {
                this.tagWholeDocumentIsLockedForModification = true;
                return this.tagWholeDocument;
            }

            set
            {
                if (!this.tagWholeDocumentIsLockedForModification)
                {
                    this.tagWholeDocument = value;
                }

                this.tagWholeDocumentIsLockedForModification = true;
            }
        }
    }

    [DataContract]
    public partial class Config
    {
        private string formatNlmToSystemXslPath;

        private string formatSystemToNlmXslPath;

        private string nlmInitialFormatXslPath;

        private string systemInitialFormatXslPath;

        private string textContentXslPath;

        [DataMember(Name = "blackListCleanXslPath")]
        public string BlackListCleanXslPath { get; set; }

        [DataMember(Name = "blackListXmlFilePath")]
        public string BlackListXmlFilePath { get; set; }

        [DataMember(Name = "codesRemoveNonCodeNodes")]
        public string CodesRemoveNonCodeNodes { get; set; }

        [DataMember(Name = "environmentsDataSourceString")]
        public string EnvironmentsDataSourceString { get; set; }

        [DataMember(Name = "envoTermsWebServiceTransformXslPath")]
        public string EnvoTermsWebServiceTransformXslPath { get; set; }

        [DataMember(Name = "floraDistrinctTaxaXslPath")]
        public string FloraDistrinctTaxaXslPath { get; set; }

        [DataMember(Name = "floraExtractedTaxaListPath")]
        public string FloraExtractedTaxaListPath { get; set; }

        [DataMember(Name = "floraExtractTaxaPartsOutputPath")]
        public string FloraExtractTaxaPartsOutputPath { get; set; }

        [DataMember(Name = "floraExtractTaxaPartsXslPath")]
        public string FloraExtractTaxaPartsXslPath { get; set; }

        [DataMember(Name = "floraExtractTaxaXslPath")]
        public string FloraExtractTaxaXslPath { get; set; }

        [DataMember(Name = "floraGenerateTemplatesXslPath")]
        public string FloraGenerateTemplatesXslPath { get; set; }

        [DataMember(Name = "floraTemplatesOutputXmlPath")]
        public string FloraTemplatesOutputXmlPath { get; set; }

        [DataMember(Name = "formatXslNlmToSystem")]
        public string FormatNlmToSystemXslPath
        {
            get
            {
                return this.formatNlmToSystemXslPath;
            }

            set
            {
                this.formatNlmToSystemXslPath = value;
                this.FormatNlmToSystemXslTransform = new XslCompiledTransform();
                this.FormatNlmToSystemXslTransform.Load(value);
            }
        }

        public XslCompiledTransform FormatNlmToSystemXslTransform { get; set; }

        [DataMember(Name = "formatXslSystemToNlm")]
        public string FormatSystemToNlmXslPath
        {
            get
            {
                return this.formatSystemToNlmXslPath;
            }

            set
            {
                this.formatSystemToNlmXslPath = value;
                this.FormatSystemToNlmXslTransform = new XslCompiledTransform();
                this.FormatSystemToNlmXslTransform.Load(value);
            }
        }

        public XslCompiledTransform FormatSystemToNlmXslTransform { get; set; }

        [DataMember(Name = "mainDictionaryDataSourceString")]
        public string MainDictionaryDataSourceString { get; set; }

        [DataMember(Name = "nlmInitialFormatXslPath")]
        public string NlmInitialFormatXslPath
        {
            get
            {
                return this.nlmInitialFormatXslPath;
            }

            set
            {
                this.nlmInitialFormatXslPath = value;
                this.NlmInitialFormatXslTransform = new XslCompiledTransform();
                this.NlmInitialFormatXslTransform.Load(value);
            }
        }

        public XslCompiledTransform NlmInitialFormatXslTransform { get; set; }

        [DataMember(Name = "rankListCleanXslPath")]
        public string RankListCleanXslPath { get; set; }

        [DataMember(Name = "rankListXmlFilePath")]
        public string RankListXmlFilePath { get; set; }

        [DataMember(Name = "referencesGetReferencesXmlPath")]
        public string ReferencesGetReferencesXmlPath { get; set; }

        [DataMember(Name = "referencesGetReferencesXslPath")]
        public string ReferencesGetReferencesXslPath { get; set; }

        [DataMember(Name = "referencesTagTemplateXmlPath")]
        public string ReferencesTagTemplateXmlPath { get; set; }

        [DataMember(Name = "referencesTagTemplateXslPath")]
        public string ReferencesTagTemplateXslPath { get; set; }

        [DataMember(Name = "systemInitialFormatXslPath")]
        public string SystemInitialFormatXslPath
        {
            get
            {
                return this.systemInitialFormatXslPath;
            }

            set
            {
                this.systemInitialFormatXslPath = value;
                this.SystemInitialFormatXslTransform = new XslCompiledTransform();
                this.SystemInitialFormatXslTransform.Load(value);
            }
        }

        public XslCompiledTransform SystemInitialFormatXslTransform { get; set; }

        [DataMember(Name = "tempDirectoryPath")]
        public string TempDirectoryPath { get; set; }

        [DataMember(Name = "textContentXslFileName")]
        public string TextContentXslPath
        {
            get
            {
                return this.textContentXslPath;
            }

            set
            {
                this.textContentXslPath = value;
                this.TextContentXslTransform = new XslCompiledTransform();
                this.TextContentXslTransform.Load(value);
            }
        }

        public XslCompiledTransform TextContentXslTransform { get; set; }

        [DataMember(Name = "whiteListCleanXslPath")]
        public string WhiteListCleanXslPath { get; set; }

        [DataMember(Name = "whiteListXmlFilePath")]
        public string WhiteListXmlFilePath { get; set; }

        [DataMember(Name = "zoobankNlmXslPath")]
        public string ZoobankNlmXslPath { get; set; }
    }
}