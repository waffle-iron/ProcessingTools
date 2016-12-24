﻿namespace ProcessingTools.Tagger.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using Contracts.Controllers;
    using ProcessingTools.Attributes;
    using ProcessingTools.Contracts;
    using ProcessingTools.Processors.Contracts;

    [Description("Custom XQuery transform.")]
    public class RunCustomXQueryTransformController : IRunCustomXQueryTransformController
    {
        private readonly IDocumentXQueryProcessor processor;

        public RunCustomXQueryTransformController(IDocumentXQueryProcessor processor)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            this.processor = processor;
        }

        public async Task<object> Run(IDocument document, IProgramSettings settings)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            int numberOfFileNames = settings.FileNames.Count;
            if (numberOfFileNames < 3)
            {
                throw new ApplicationException("The name of the XQuey file should be set.");
            }

            this.processor.XQueryFileFullName = settings.FileNames[2];

            await this.processor.Process(document);

            return true;
        }
    }
}