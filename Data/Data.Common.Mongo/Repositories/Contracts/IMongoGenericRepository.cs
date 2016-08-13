﻿namespace ProcessingTools.Data.Common.Mongo.Repositories.Contracts
{
    using ProcessingTools.Data.Common.Repositories.Contracts;

    public interface IMongoGenericRepository<T> : IMongoSearchableRepository<T>, IGenericRepository<T>
        where T : class
    {
    }
}