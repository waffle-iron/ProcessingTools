﻿namespace ProcessingTools.BaseLibrary
{
    using System;
    using Configurator;
    using Globals;

    public class XPathProvider : IXPathProvider
    {
        private Config config;

        /// <summary>
        /// Creates new XPathProvider object.
        /// </summary>
        /// <param name="config">Required to obtain settings.</param>
        /// <exception cref="Systesm.NullReferenceException">Config config must not be null.</exception>
        public XPathProvider(Config config)
        {
            this.Config = config;
        }

        public Config Config
        {
            get
            {
                return this.config;
            }

            private set
            {
                if (value == null)
                {
                    throw new NullReferenceException("Config object should not be null.");
                }
                else
                {
                    this.config = value;
                }
            }
        }

        public string SelectContentNodesXPath
        {
            get
            {
                switch (this.Config.ArticleSchemaType)
                {
                    case SchemaType.Nlm:
                        return "//p|//license-p|//li|//th|//td|//mixed-citation|//element-citation|//nlm-citation|//tp:nomenclature-citation";

                    default:
                        // TODO
                        return "//p|//li|//th|//td";
                }
            }
        }

        public string SelectContentNodesXPathTemplate
        {
            get
            {
                switch (this.Config.ArticleSchemaType)
                {
                    case SchemaType.Nlm:
                        return "//p[{0}]|//license-p[{0}]|//li[{0}]|//th[{0}]|//td[{0}]|//mixed-citation[{0}]|//element-citation[{0}]|//nlm-citation[{0}]|//tp:nomenclature-citation[{0}]";

                    default:
                        // TODO
                        return "//p[{0}]|//li[{0}]|//th[{0}]|//td[{0}]";
                }
            }
        }
    }
}
