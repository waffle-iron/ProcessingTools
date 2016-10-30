﻿namespace ProcessingTools.Data.Common.File.Repositories.Contracts
{
    using ProcessingTools.Contracts.Data.Repositories;

    public interface IFileCrudRepository<T> : ICrudRepository<T>, IFileSearchableRepository<T>, IFileRepository<T>
        where T : class
    {
    }
}
