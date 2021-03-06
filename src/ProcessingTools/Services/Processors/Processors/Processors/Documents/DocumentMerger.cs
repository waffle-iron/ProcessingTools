﻿namespace ProcessingTools.Processors.Processors.Documents
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts;
    using ProcessingTools.Enumerations;
    using ProcessingTools.Processors.Contracts.Processors.Documents;
    using ProcessingTools.Xml.Contracts.Wrappers;

    public class DocumentMerger : IDocumentMerger
    {
        private readonly IDocumentReader documentReader;
        private readonly IDocumentWrapper documentWrapper;

        public DocumentMerger(IDocumentReader documentReader, IDocumentWrapper documentWrapper)
        {
            this.documentReader = documentReader ?? throw new ArgumentNullException(nameof(documentReader));
            this.documentWrapper = documentWrapper ?? throw new ArgumentNullException(nameof(documentWrapper));
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

            var document = this.documentWrapper.Create();

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
