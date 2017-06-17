﻿namespace ProcessingTools.Geo.Services.Data.Entity.Services
{
    using ProcessingTools.Contracts.Data.Repositories.Geo;
    using ProcessingTools.Contracts.Filters.Geo;
    using ProcessingTools.Contracts.Models.Geo;
    using ProcessingTools.Contracts.Services.Data.Geo.Services;
    using ProcessingTools.Geo.Services.Data.Abstractions;

    public class EntityGeoEpithetsDataService : AbstractGeoDataService<IGeoEpithetsRepository, IGeoEpithet, IGeoEpithetsFilter>, IGeoEpithetsDataService
    {
        public EntityGeoEpithetsDataService(IGeoEpithetsRepository repository)
            : base(repository)
        {
        }
    }
}
