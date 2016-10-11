﻿namespace ProcessingTools.BaseLibrary
{
    using ProcessingTools.Xml.Cache;
    using ProcessingTools.Xml.Providers;
    using ProcessingTools.Xml.Transformers;

    /// <summary>
    /// This class provides extension methods for transformation of TaxPub NLM to system XML schema.
    /// </summary>
    public static class XmlNormalizer
    {
        /// <summary>
        /// Transforms a given XML string to system Xml Schema.
        /// </summary>
        /// <param name="xml">XML as string to be transformed.</param>
        /// <returns>Transformed XML as string.</returns>
        public static string NormalizeXmlToSystemXml(this string xml)
        {
            // TODO: DI, async
            var transformer = new XslTransformer(new FormatNlmToSystemXslTransformProvider(new XslTransformCache()));
            return transformer.Transform(xml).Result;
        }

        /// <summary>
        /// Transforms a given XML string to TaxPub NLM Xml Schema.
        /// </summary>
        /// <param name="xml">XML as string to be transformed.</param>
        /// <returns>Transformed XML as string.</returns>
        private static string NormalizeXmlToNlmXml(this string xml)
        {
            // TODO: DI, async
            var transformer = new XslTransformer(new FormatSystemToNlmXslTransformProvider(new XslTransformCache()));
            return transformer.Transform(xml).Result;
        }
    }
}
