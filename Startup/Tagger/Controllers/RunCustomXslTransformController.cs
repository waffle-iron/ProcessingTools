﻿namespace ProcessingTools.MainProgram.Controllers
{
    using System.Threading.Tasks;
    using System.Xml;

    using Contracts;
    using Factories;
    using ProcessingTools.BaseLibrary;
    using ProcessingTools.Contracts;

    public class RunCustomXslTransformController : TaggerControllerFactory, IRunCustomXslTransformController
    {
        protected override async Task Run(XmlDocument document, XmlNamespaceManager namespaceManager, ProgramSettings settings, ILogger logger)
        {
            var processor = new CustomXslRunner(settings.QueryFileName, document.OuterXml);

            await processor.Process();

            document.LoadXml(processor.Xml);
        }
    }
}