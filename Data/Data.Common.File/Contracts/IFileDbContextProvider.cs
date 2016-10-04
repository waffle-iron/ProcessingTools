﻿namespace ProcessingTools.Data.Common.File.Contracts
{
    using ProcessingTools.Data.Common.Contracts;

    public interface IFileDbContextProvider<TContext, T> : IDatabaseProvider<TContext>
        where TContext : IFileDbContext<T>
    {
    }
}