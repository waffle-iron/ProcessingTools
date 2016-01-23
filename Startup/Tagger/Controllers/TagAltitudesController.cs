﻿namespace ProcessingTools.MainProgram.Controllers
{
    using System;
    using System.Xml;

    using Contracts;
    using Factories;
    using ProcessingTools.Data.Miners.Common.Contracts;
    using ProcessingTools.Data.Miners.Contracts;

    public class TagAltitudesController : StringTaggerControllerFactory, ITagAltitudesController
    {
        private readonly IAltitudesDataMiner miner;
        private readonly XmlElement tagModel;

        public TagAltitudesController(IAltitudesDataMiner miner)
        {
            if (miner == null)
            {
                throw new ArgumentNullException("miner");
            }

            this.miner = miner;

            XmlDocument document = new XmlDocument();
            this.tagModel = document.CreateElement("named-content");
            this.tagModel.SetAttribute("content-type", "altitude");
        }

        protected override IStringDataMiner Miner => this.miner;

        protected override XmlElement TagModel => this.tagModel;
    }
}