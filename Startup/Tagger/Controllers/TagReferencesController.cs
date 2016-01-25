﻿namespace ProcessingTools.MainProgram.Controllers
{
    using System.Threading.Tasks;
    using System.Xml;

    using Contracts;
    using Factories;
    using ProcessingTools.BaseLibrary.References;
    using ProcessingTools.Contracts;

    public class TagReferencesController : TaggerControllerFactory, ITagReferencesController
    {
        protected override async Task Run(XmlDocument document, XmlNamespaceManager namespaceManager, ProgramSettings settings, ILogger logger)
        {
            var tagger = new ReferencesTagger(settings.Config, document.OuterXml, logger);

            await tagger.Tag();

            document.LoadXml(tagger.Xml);
        }
    }
}