﻿namespace ProcessingTools.Data.Common.Entity.Repositories.Contracts
{
    using ProcessingTools.Contracts.Data.Repositories;

    public interface IEntityGenericRepository<T> : ICrudRepository<T>, IEntityCrudRepository<T>, IEntitySearchableRepository<T>, IEntityRepository<T>
        where T : class
    {
    }
}
