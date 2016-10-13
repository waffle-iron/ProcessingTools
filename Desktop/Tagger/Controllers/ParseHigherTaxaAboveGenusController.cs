﻿namespace ProcessingTools.Tagger.Controllers
{
    using Contracts;

    using ProcessingTools.Attributes;
    using ProcessingTools.Bio.Taxonomy.Contracts;
    using ProcessingTools.Bio.Taxonomy.Processors.Contracts.Parsers;
    using ProcessingTools.Bio.Taxonomy.Services.Data.Contracts;
    using ProcessingTools.Contracts;

    [Description("Make higher taxa of type 'above-genus'.")]
    public class ParseHigherTaxaAboveGenusController : ParseHigherTaxaWithDataServiceGenericController<IAboveGenusTaxaRankResolverDataService>, IParseHigherTaxaAboveGenusController
    {
        public ParseHigherTaxaAboveGenusController(
            IDocumentFactory documentFactory,
            IHigherTaxaParserWithDataService<IAboveGenusTaxaRankResolverDataService, ITaxonRank> parser,
            ILogger logger)
            : base(documentFactory, parser, logger)
        {
        }
    }
}
