﻿namespace ProcessingTools.Processors.Processors.Quantities
{
    using ProcessingTools.Data.Miners.Contracts.Miners.Quantities;
    using ProcessingTools.Layout.Processors.Contracts.Taggers;
    using ProcessingTools.Processors.Contracts;
    using ProcessingTools.Processors.Contracts.Processors.Quantities;
    using ProcessingTools.Processors.Contracts.Providers;
    using ProcessingTools.Processors.Generics;

    public class QuantitiesTagger : GenericStringMinerTagger<IQuantitiesDataMiner, IQuantityTagModelProvider>, IQuantitiesTagger
    {
        public QuantitiesTagger(IGenericStringDataMinerEvaluator<IQuantitiesDataMiner> evaluator, IStringTagger tagger, IQuantityTagModelProvider tagModelProvider)
            : base(evaluator, tagger, tagModelProvider)
        {
        }
    }
}
