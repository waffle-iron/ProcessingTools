﻿namespace ProcessingTools.Processors.Bio
{
    using Contracts;
    using Contracts.Bio;
    using Contracts.Providers;
    using Generics;
    using ProcessingTools.Bio.Data.Miners.Contracts;
    using ProcessingTools.Layout.Processors.Contracts.Taggers;

    public class MorphologicalEpithetsTagger : GenericStringMinerTagger<IMorphologicalEpithetsDataMiner, IMorphologicalEpithetTagModelProvider>, IMorphologicalEpithetsTagger
    {
        public MorphologicalEpithetsTagger(IGenericStringDataMinerEvaluator<IMorphologicalEpithetsDataMiner> evaluator, IStringTagger tagger, IMorphologicalEpithetTagModelProvider tagModelProvider)
            : base(evaluator, tagger, tagModelProvider)
        {
        }
    }
}