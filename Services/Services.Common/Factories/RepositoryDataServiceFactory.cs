﻿namespace ProcessingTools.Services.Common.Factories
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using ProcessingTools.Common.Exceptions;
    using ProcessingTools.Constants;
    using ProcessingTools.Contracts.Data.Repositories;
    using ProcessingTools.Extensions.Linq;

    public abstract class RepositoryDataServiceFactory<TDbModel, TServiceModel> : RepositoryDataServiceFactoryBase<TDbModel, TServiceModel>
    {
        protected abstract Expression<Func<TDbModel, TServiceModel>> MapDbModelToServiceModel { get; }

        protected abstract Expression<Func<TServiceModel, TDbModel>> MapServiceModelToDbModel { get; }

        protected abstract Expression<Func<TDbModel, object>> SortExpression { get; }

        public override async Task<IQueryable<TServiceModel>> All(ISearchableCountableCrudRepository<TDbModel> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            var result = await repository.Query
                .Select(this.MapDbModelToServiceModel)
                .ToListAsync();

            return result.AsQueryable();
        }

        public override async Task<IQueryable<TServiceModel>> Query(ISearchableCountableCrudRepository<TDbModel> repository, int skip, int take)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (skip < 0)
            {
                throw new InvalidSkipValuePagingException();
            }

            if (1 > take || take > PagingConstants.MaximalItemsPerPageAllowed)
            {
                throw new InvalidTakeValuePagingException();
            }

            var result = await repository.Query
                .OrderBy(this.SortExpression)
                .Skip(skip)
                .Take(take)
                .Select(this.MapDbModelToServiceModel)
                .ToListAsync();

            return result.AsQueryable();
        }

        public override async Task<IQueryable<TServiceModel>> Get(ISearchableCountableCrudRepository<TDbModel> repository, params object[] ids)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (ids == null || ids.Length < 1)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            var mapping = this.MapDbModelToServiceModel.Compile();

            var result = new ConcurrentQueue<TServiceModel>();

            foreach (var id in ids)
            {
                var entity = await repository.Get(id);
                result.Enqueue(mapping.Invoke(entity));
            }

            return new HashSet<TServiceModel>(result).AsQueryable();
        }

        protected override IEnumerable<TDbModel> MapServiceModelsToEntities(TServiceModel[] models)
        {
            return models.AsQueryable()
                .Select(this.MapServiceModelToDbModel)
                .ToList();
        }
    }
}
