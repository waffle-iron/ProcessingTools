﻿namespace ProcessingTools.Processors.Processors.Documents
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts.Documents;
    using ProcessingTools.Contracts;
    using ProcessingTools.Contracts.Types;
    using ProcessingTools.Xml.Contracts.Providers;

    public class DocumentMerger : IDocumentMerger
    {
        private readonly IDocumentFactory documentFactory;
        private readonly IDocumentReader documentReader;
        private readonly IDocumentWrapperProvider documentWrapperProvider;

        public DocumentMerger(IDocumentFactory documentFactory, IDocumentReader documentReader, IDocumentWrapperProvider documentWrapperProvider)
        {
            if (documentFactory == null)
            {
                throw new ArgumentNullException(nameof(documentFactory));
            }

            if (documentReader == null)
            {
                throw new ArgumentNullException(nameof(documentReader));
            }

            if (documentWrapperProvider == null)
            {
                throw new ArgumentNullException(nameof(documentWrapperProvider));
            }

            this.documentFactory = documentFactory;
            this.documentReader = documentReader;
            this.documentWrapperProvider = documentWrapperProvider;
        }

        public async Task<IDocument> Merge(params string[] fileNames)
        {
            if (fileNames == null || fileNames.Length < 1)
            {
                throw new ArgumentNullException(nameof(fileNames));
            }

            var cleanedFileNames = fileNames.Where(fn => !string.IsNullOrWhiteSpace(fn))
                .Distinct()
                .ToArray();

            if (cleanedFileNames.Length < 1)
            {
                throw new ArgumentException("No valid file names are provided", nameof(fileNames));
            }

            var document = this.documentFactory.Create(this.documentWrapperProvider.DocumentWrapper);

            foreach (var fileName in cleanedFileNames)
            {
                var readDocument = await this.documentReader.ReadDocument(fileName);
                var fragment = document.XmlDocument.CreateDocumentFragment();
                fragment.InnerXml = readDocument.XmlDocument.DocumentElement.OuterXml;
                document.XmlDocument.DocumentElement.AppendChild(fragment);
            }

            document.SchemaType = SchemaType.System;

            return document;
        }
    }
}