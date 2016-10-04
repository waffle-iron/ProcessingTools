﻿namespace ProcessingTools.Tagger.Controllers
{
    using System;
    using System.Xml;

    using Contracts;
    using Factories;

    using ProcessingTools.Attributes;
    using ProcessingTools.Data.Miners.Common.Contracts;
    using ProcessingTools.Data.Miners.Contracts;

    [Description("Tag products.")]
    public class TagProductsController : StringTaggerControllerFactory, ITagProductsController
    {
        private readonly IProductsDataMiner miner;
        private readonly XmlElement tagModel;

        public TagProductsController(IProductsDataMiner miner)
        {
            if (miner == null)
            {
                throw new ArgumentNullException(nameof(miner));
            }

            this.miner = miner;

            XmlDocument document = new XmlDocument();
            this.tagModel = document.CreateElement("named-content");
            this.tagModel.SetAttribute("content-type", "product");
        }

        protected override IStringDataMiner Miner => this.miner;

        protected override XmlElement TagModel => this.tagModel;
    }
}