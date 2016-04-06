﻿namespace ProcessingTools.Documents.Services.Data
{
    using System;
    using System.Collections.Generic;

    using Contracts;
    using Models;

    using ProcessingTools.Documents.Data.Common.Repositories.Contracts;
    using ProcessingTools.Documents.Data.Models;
    using ProcessingTools.Services.Common.Factories;

    public class CountriesDataService : GenericRepositoryDataServiceFactory<Country, CountryServiceModel>, ICountriesDataService
    {
        public CountriesDataService(IDocumentsRepository<Country> repository)
            : base(repository)
        {
        }

        protected override IEnumerable<Country> MapServiceModelToDbModel(params CountryServiceModel[] models)
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            var result = new HashSet<Country>();
            foreach (var model in models)
            {
                var entity = new Country
                {
                    Name = model.Name
                };

                result.Add(entity);
            }

            return result;
        }

        protected override IEnumerable<CountryServiceModel> MapDbModelToServiceModel(params Country[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var result = new HashSet<CountryServiceModel>();
            foreach (var entity in entities)
            {
                var model = new CountryServiceModel
                {
                    Id = entity.Id,
                    Name = entity.Name
                };

                result.Add(model);
            }

            return result;
        }
    }
}