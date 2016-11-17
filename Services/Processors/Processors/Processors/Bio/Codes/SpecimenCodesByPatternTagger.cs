﻿namespace ProcessingTools.Processors.Bio.Codes
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts.Bio.Codes;
    using Models.Bio.Codes;
    using ProcessingTools.Bio.Data.Miners.Contracts.SpecimenCodes;
    using ProcessingTools.Constants.Schema;
    using ProcessingTools.Contracts;
    using ProcessingTools.Harvesters.Contracts.Content;
    using ProcessingTools.Layout.Processors.Contracts.Taggers;
    using ProcessingTools.Layout.Processors.Models.Taggers;

    public class SpecimenCodesByPatternTagger : ISpecimenCodesByPatternTagger
    {
        private readonly ITextContentHarvester contentHarvester;
        private readonly ISpecimenCodesByPatternDataMiner miner;
        private readonly ISimpleXmlSerializableObjectTagger<SpecimenCodeSerializableModel> tagger;

        public SpecimenCodesByPatternTagger(
            ITextContentHarvester contentHarvester,
            ISpecimenCodesByPatternDataMiner miner,
            ISimpleXmlSerializableObjectTagger<SpecimenCodeSerializableModel> tagger)
        {
            if (contentHarvester == null)
            {
                throw new ArgumentNullException(nameof(contentHarvester));
            }

            if (miner == null)
            {
                throw new ArgumentNullException(nameof(miner));
            }

            if (tagger == null)
            {
                throw new ArgumentNullException(nameof(tagger));
            }

            this.contentHarvester = contentHarvester;
            this.miner = miner;
            this.tagger = tagger;
        }

        public async Task<object> Tag(IDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var textContent = await this.contentHarvester.Harvest(document.XmlDocument.DocumentElement);
            var data = (await this.miner.Mine(textContent))
                .ToArray();

            var specimenCodes = data.Select(s => new SpecimenCodeSerializableModel
            {
                SpecificUse = s.ContentType,
                Value = s.Content
            });

            var settings = new ContentTaggerSettings
            {
                CaseSensitive = true,
                MinimalTextSelect = true
            };

            return await this.tagger.Tag(
                document.XmlDocument.DocumentElement,
                document.NamespaceManager,
                specimenCodes,
                XPathStrings.RootNodesOfContext,
                settings);
        }
    }
}
