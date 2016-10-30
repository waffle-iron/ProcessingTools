﻿namespace ProcessingTools.Bio.Data.Repositories
{
    using System;
    using Contracts;
    using ProcessingTools.Bio.Data.Contracts;
    using ProcessingTools.Contracts.Data.Repositories;

    public class BioDataRepositoryProvider<T> : IBioDataRepositoryProvider<T>
        where T : class
    {
        private readonly IBioDbContextProvider contextProvider;

        public BioDataRepositoryProvider(IBioDbContextProvider contextProvider)
        {
            if (contextProvider == null)
            {
                throw new ArgumentNullException(nameof(contextProvider));
            }

            this.contextProvider = contextProvider;
        }

        public IGenericRepository<T> Create()
        {
            return new BioDataRepository<T>(this.contextProvider);
        }
    }
}