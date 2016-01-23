﻿namespace ProcessingTools.MainProgram.Contracts
{
    using System.Threading.Tasks;
    using System.Xml;

    using ProcessingTools.Contracts;

    public interface ITaggerController
    {
        Task Run(XmlNode context, XmlNamespaceManager namespaceManager,  ProgramSettings settings, ILogger logger);
    }
}