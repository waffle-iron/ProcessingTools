﻿namespace ProcessingTools.Services.Data.Services
{
    using AutoMapper;

    using Contracts;
    using Models.Contracts;

    using ProcessingTools.Data.Models;
    using ProcessingTools.Data.Repositories.Contracts;
    using ProcessingTools.Services.Common.Factories;

    public class InstitutionsDataService : EfGenericCrudDataServiceFactory<Institution, IInstitution, int>, IInstitutionsDataService
    {
        public InstitutionsDataService(IDataRepository<Institution> repository)
            : base(repository, i => i.Name.Length)
        {
            Mapper.CreateMap<Institution, IInstitution>();
            Mapper.CreateMap<IInstitution, Institution>();
        }
    }
}