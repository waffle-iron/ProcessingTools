﻿namespace ProcessingTools.Geo.Services.Data.Services
{
    using System;
    using System.Linq.Expressions;
    using ProcessingTools.Contracts.Filters;
    using ProcessingTools.Geo.Data.Entity.Contracts.Repositories;
    using ProcessingTools.Geo.Data.Entity.Models;
    using ProcessingTools.Geo.Services.Data.Contracts;
    using ProcessingTools.Geo.Services.Data.Models;
    using ProcessingTools.Services.Abstractions;

    public class GeoEpithetsDataService : AbstractMultiDataServiceAsync<GeoEpithet, GeoEpithetServiceModel, IFilter>, IGeoEpithetsDataService
    {
        public GeoEpithetsDataService(IGeoDataRepository<GeoEpithet> repository)
            : base(repository)
        {
        }

        protected override Expression<Func<GeoEpithet, GeoEpithetServiceModel>> MapEntityToModel => e => new GeoEpithetServiceModel
        {
            Id = e.Id,
            Name = e.Name
        };

        protected override Expression<Func<GeoEpithetServiceModel, GeoEpithet>> MapModelToEntity => m => new GeoEpithet
        {
            Id = m.Id,
            Name = m.Name
        };
    }
}
