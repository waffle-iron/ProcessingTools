﻿namespace ProcessingTools.Geo.Services.Data.Entity.Services
{
    using ProcessingTools.Contracts.Data.Repositories.Geo;
    using ProcessingTools.Contracts.Filters.Geo;
    using ProcessingTools.Contracts.Models.Geo;
    using ProcessingTools.Contracts.Services.Data.Geo.Services;
    using ProcessingTools.Geo.Services.Data.Abstractions;

    public class RegionsDataService : AbstractGeoSynonymisableDataService<IRegionsRepository, IRegion, IRegionsFilter, IRegionSynonym, IRegionSynonymsFilter>, IRegionsDataService
    {
        public RegionsDataService(IRegionsRepository repository)
            : base(repository)
        {
        }
    }
}
