﻿namespace ProcessingTools.MainProgram.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;

    using Attributes;
    using BaseLibrary;
    using Contracts;
    using Factories;
    using Models;
    using ProcessingTools.Contracts;
    using ProcessingTools.Data.Miners.Contracts;
    using ProcessingTools.Extensions;

    public class TagWebLinksController : TaggerControllerFactory, ITagWebLinksController
    {
        private const string XPath = "/*";
        private readonly INlmExternalLinksDataMiner miner;

        public TagWebLinksController(INlmExternalLinksDataMiner miner)
        {
            if (miner == null)
            {
                throw new ArgumentNullException("miner");
            }

            this.miner = miner;
        }

        protected override async Task Run(XmlDocument document, XmlNamespaceManager namespaceManager, ProgramSettings settings, ILogger logger)
        {
            var textContent = document.GetTextContent(settings.Config.TextContentXslTransform);
            var data = (await this.miner.Mine(textContent))
                .Select(i => new ExternalLinkSerializableModel
                {
                    ExternalLinkType = i.Type.GetValue(),
                    Value = i.Content
                });

            var tagger = new SimpleXmlSerializableObjectTagger<ExternalLinkSerializableModel>(document.OuterXml, data, XPath, namespaceManager, false, true, logger);

            await tagger.Tag();

            document.LoadXml(tagger.Xml);
        }
    }
}
