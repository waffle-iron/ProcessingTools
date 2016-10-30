﻿namespace ProcessingTools.Services.Common.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Contracts;

    using ProcessingTools.Contracts.Data.Repositories;
    using ProcessingTools.Extensions;

    public abstract class MultiDataServiceWithRepositoryProviderFactory<TDbModel, TServiceModel> : RepositoryMultiDataServiceFactory<TDbModel, TServiceModel>, IMultiEntryDataService<TServiceModel>, IDisposable
    {
        private readonly IGenericRepositoryProvider<TDbModel> repositoryProvider;

        public MultiDataServiceWithRepositoryProviderFactory(IGenericRepositoryProvider<TDbModel> repositoryProvider)
        {
            if (repositoryProvider == null)
            {
                throw new ArgumentNullException(nameof(repositoryProvider));
            }

            this.repositoryProvider = repositoryProvider;
        }

        protected override abstract Expression<Func<TDbModel, IEnumerable<TServiceModel>>> MapDbModelToServiceModel { get; }

        protected override abstract Expression<Func<TServiceModel, IEnumerable<TDbModel>>> MapServiceModelToDbModel { get; }

        protected override abstract Expression<Func<TDbModel, object>> SortExpression { get; }

        public virtual async Task<object> Add(params TServiceModel[] models)
        {
            var repository = this.repositoryProvider.Create();

            var savedItems = await base.Add(repository: repository, models: models);

            repository.TryDispose();

            return savedItems;
        }

        public virtual async Task<IQueryable<TServiceModel>> All()
        {
            var repository = this.repositoryProvider.Create();

            var result = await base.All(repository: repository);

            repository.TryDispose();

            return result;
        }

        public virtual async Task<IQueryable<TServiceModel>> Query(int skip, int take)
        {
            var repository = this.repositoryProvider.Create();

            var result = await base.Query(repository: repository, skip: skip, take: take);

            repository.TryDispose();

            return result;
        }

        public virtual async Task<object> Delete(params object[] ids)
        {
            var repository = this.repositoryProvider.Create();

            var savedItems = await base.Delete(repository: repository, ids: ids);

            repository.TryDispose();

            return savedItems;
        }

        public virtual async Task<object> Delete(params TServiceModel[] models)
        {
            var repository = this.repositoryProvider.Create();

            var savedItems = await base.Delete(repository: repository, models: models);

            repository.TryDispose();

            return savedItems;
        }

        public virtual async Task<IQueryable<TServiceModel>> Get(params object[] ids)
        {
            var repository = this.repositoryProvider.Create();

            var result = await base.Get(repository: repository, ids: ids);

            repository.TryDispose();

            return result;
        }

        public virtual async Task<object> Update(params TServiceModel[] models)
        {
            var repository = this.repositoryProvider.Create();

            var savedItems = await base.Update(repository: repository, models: models);

            repository.TryDispose();

            return savedItems;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.repositoryProvider.TryDispose();
            }
        }
    }
}
