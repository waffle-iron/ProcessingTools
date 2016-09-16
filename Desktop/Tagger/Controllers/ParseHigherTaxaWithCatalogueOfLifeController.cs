﻿namespace ProcessingTools.Tagger.Controllers
{
    using Contracts;

    using ProcessingTools.Attributes;
    using ProcessingTools.BaseLibrary.Taxonomy.Contracts;
    using ProcessingTools.Bio.Taxonomy.Contracts;
    using ProcessingTools.Bio.Taxonomy.Services.Data.Contracts;

    [Description("Parse higher taxa using CoL.")]
    public class ParseHigherTaxaWithCatalogueOfLifeController : ParseHigherTaxaWithDataServiceGenericController<ICatalogueOfLifeTaxaRankResolverDataService>, IParseHigherTaxaWithCatalogueOfLifeController
    {
        public ParseHigherTaxaWithCatalogueOfLifeController(IHigherTaxaParserWithDataService<ICatalogueOfLifeTaxaRankResolverDataService, ITaxonRank> parser)
            : base(parser)
        {
        }
    }
}
