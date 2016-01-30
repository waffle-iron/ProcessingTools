﻿namespace ProcessingTools.Geo.Services.Data.Services
{
    using AutoMapper;

    using Contracts;
    using Models.Contracts;

    using ProcessingTools.Geo.Data.Models;
    using ProcessingTools.Geo.Data.Repositories.Contracts;
    using ProcessingTools.Services.Common.Factories;

    public class GeoNamesDataService : EfGenericCrudDataServiceFactory<GeoName, IGeoName, int>, IGeoNamesDataService
    {
        public GeoNamesDataService(IGeoDataRepository<GeoName> repository)
            : base(repository, e => e.Name.Length)
        {
            Mapper.CreateMap<GeoName, IGeoName>();
            Mapper.CreateMap<IGeoName, GeoName>();
        }
    }
}