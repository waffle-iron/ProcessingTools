﻿namespace ProcessingTools.BaseLibrary
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;

    using ProcessingTools.Contracts;
    using ProcessingTools.DocumentProvider;
    using ProcessingTools.Xml.Extensions;

    public class StringTagger : TaxPubDocument, ITagger
    {
        private string contentNodesXPathTemplate;
        private XmlNamespaceManager namespaceManager;
        private XmlElement tagModel;
        private XmlDocumentFragment bufferXml;
        private IQueryable<string> data;
        private ILogger logger;

        public StringTagger(string xml, IQueryable<string> data, XmlElement tagModel, string contentNodesXPathTemplate, XmlNamespaceManager namespaceManager, ILogger logger)
            : base(xml)
        {
            this.data = data;
            this.tagModel = tagModel;
            this.contentNodesXPathTemplate = contentNodesXPathTemplate;
            this.namespaceManager = namespaceManager;
            this.logger = logger;

            this.bufferXml = this.XmlDocument.CreateDocumentFragment();
        }

        public async Task Tag()
        {
            await this.data.ToList()
                .Select(this.XmlEncode)
                .OrderByDescending(i => i.Length)
                .TagContentInDocument(
                    this.tagModel,
                    this.contentNodesXPathTemplate,
                    this,
                    false,
                    true,
                    this.logger);
        }

        private string XmlEncode(string text)
        {
            this.bufferXml.InnerText = text;
            return this.bufferXml.InnerXml;
        }
    }
}
