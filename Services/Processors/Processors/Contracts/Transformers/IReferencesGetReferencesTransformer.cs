﻿namespace ProcessingTools.Processors.Contracts.Transformers
{
    using ProcessingTools.Xml.Contracts.Providers;
    using ProcessingTools.Xml.Contracts.Transformers;

    public interface IReferencesGetReferencesTransformer : IXslTransformer<IReferencesGetReferencesXslTransformProvider>, IXmlTransformer
    {
    }
}