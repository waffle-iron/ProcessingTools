﻿namespace ProcessingTools.Processors.Geo
{
    using Contracts;
    using Contracts.Geo;
    using Contracts.Providers;
    using Generics;
    using ProcessingTools.Geo.Data.Miners.Contracts;
    using ProcessingTools.Layout.Processors.Contracts.Taggers;

    public class GeoNamesTagger : GenericStringMinerTagger<IGeoNamesDataMiner, IGeoNameTagModelProvider>, IGeoNamesTagger
    {
        public GeoNamesTagger(IGenericStringDataMinerEvaluator<IGeoNamesDataMiner> evaluator, IStringTagger tagger, IGeoNameTagModelProvider tagModelProvider)
            : base(evaluator, tagger, tagModelProvider)
        {
        }
    }
}