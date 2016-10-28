﻿namespace ProcessingTools.Processors.Bio
{
    using Contracts;
    using Contracts.Bio;
    using Contracts.Providers;
    using Generics;
    using ProcessingTools.Bio.Data.Miners.Contracts;
    using ProcessingTools.Layout.Processors.Contracts.Taggers;

    public class TypeStatusTagger : GenericStringMinerTagger<ITypeStatusDataMiner, ITypeStatusTagModelProvider>, ITypeStatusTagger
    {
        public TypeStatusTagger(IGenericStringDataMinerEvaluator<ITypeStatusDataMiner> evaluator, IStringTagger tagger, ITypeStatusTagModelProvider tagModelProvider)
            : base(evaluator, tagger, tagModelProvider)
        {
        }
    }
}