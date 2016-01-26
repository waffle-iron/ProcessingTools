﻿namespace ProcessingTools.MainProgram.Controllers
{
    using System.Threading.Tasks;
    using System.Xml;

    using Contracts;
    using Factories;
    using ProcessingTools.Attributes;
    using ProcessingTools.BaseLibrary.Floats;
    using ProcessingTools.Contracts;

    [Description("Tag table foot-notes.")]
    public class TagTableFootnoteController : TaggerControllerFactory, ITagTableFootnoteController
    {
        protected override async Task Run(XmlDocument document, XmlNamespaceManager namespaceManager, ProgramSettings settings, ILogger logger)
        {
            var tagger = new TableFootNotesTagger(settings.Config, document.OuterXml, logger);

            await tagger.Tag();

            document.LoadXml(tagger.Xml);
        }
    }
}