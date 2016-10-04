﻿namespace ProcessingTools.Services.Cache
{
    using Contracts;
    using Models;

    using ProcessingTools.Cache.Data.Models;
    using ProcessingTools.Cache.Data.Repositories.Contracts;

    public class ValidationCacheService : SimpleCacheService<ValidationCacheEntity, ValidationCacheServiceModel>, IValidationCacheService
    {
        public ValidationCacheService(IValidationCacheDataRepository repository)
            : base(repository)
        {
        }
    }
}