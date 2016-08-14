﻿namespace ProcessingTools.Data.Common.Mongo.Repositories.Contracts
{
    using ProcessingTools.Data.Common.Repositories.Contracts;

    public interface IMongoGenericRepository<T> : IMongoSearchableRepository<T>, IMongoCrudRepository<T>, IMongoIterableRepository<T>, IGenericRepository<T>
        where T : class
    {
    }
}