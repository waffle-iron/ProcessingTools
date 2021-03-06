﻿namespace ProcessingTools.Layout.Processors.Abstractions.Normalizers
{
    using System;
    using System.Threading.Tasks;
    using System.Xml;

    using ProcessingTools.Contracts;

    public abstract class AbstractXmlNormalizer : IXmlContextNormalizer
    {
        private readonly IXmlTransformer transformer;

        public AbstractXmlNormalizer(IXmlTransformer transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            this.transformer = transformer;
        }

        public Task<string> Normalize(XmlNode context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return this.transformer.Transform(context);
        }
    }
}
