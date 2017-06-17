﻿namespace ProcessingTools.Geo.Services.Data.Entity.Services
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using ProcessingTools.Common.Extensions.Linq;
    using ProcessingTools.Contracts.Filters.Geo;
    using ProcessingTools.Contracts.Models.Geo;
    using ProcessingTools.Contracts.Services;
    using ProcessingTools.Enumerations;
    using ProcessingTools.Geo.Data.Entity.Contracts.Repositories;
    using ProcessingTools.Geo.Data.Entity.Models;
    using ProcessingTools.Geo.Services.Data.Entity.Abstractions;
    using ProcessingTools.Geo.Services.Data.Entity.Contracts.Services;

    public class EntityGeoNamesRepository : AbstractGeoRepository<GeoName, IGeoName, IGeoNamesFilter>, IEntityGeoNamesRepository
    {
        private readonly Func<GeoName, IGeoName> mapEntityToModel;

        public EntityGeoNamesRepository(IGeoRepository<GeoName> repository, IEnvironment environment)
            : base(repository, environment)
        {
            this.mapEntityToModel = this.MapEntityToModelExpression.Compile();
        }

        protected override Func<GeoName, IGeoName> MapEntityToModel => this.mapEntityToModel;

        protected override Func<IGeoName, GeoName> MapModelToEntity => m => new GeoName
        {
            Id = m.Id,
            Name = m.Name
        };

        private Expression<Func<GeoName, IGeoName>> MapEntityToModelExpression => m => new ProcessingTools.Geo.Services.Data.Entity.Models.GeoNameModel
        {
            Id = m.Id,
            Name = m.Name
        };

        public override async Task<IGeoName[]> SelectAsync(IGeoNamesFilter filter)
        {
            var query = this.GetQuery(filter)
                .Select(this.MapEntityToModelExpression);

            return await query.ToArrayAsync();
        }

        public override async Task<IGeoName[]> SelectAsync(IGeoNamesFilter filter, int skip, int take, string sortColumn, SortOrder sortOrder = SortOrder.Ascending)
        {
            var query = this.GetQuery(filter)
                .OrderByName(sortColumn, sortOrder)
                .Skip(skip)
                .Take(take)
                .Select(this.MapEntityToModelExpression);

            return await query.ToArrayAsync();
        }

        public override async Task<object> UpdateAsync(IGeoName model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var entity = this.Repository.Get(model.Id);
            if (entity == null)
            {
                return null;
            }

            entity.Name = model.Name;

            return await this.UpdateEntityAsync(entity);
        }

        protected override IQueryable<GeoName> GetQuery(IGeoNamesFilter filter)
        {
            var query = this.Repository.Queryable();

            if (filter != null)
            {
                query = query.Where(
                    n =>
                        (!filter.Id.HasValue || n.Id == filter.Id) &&
                        (string.IsNullOrEmpty(filter.Name) || n.Name.ToLower().Contains(filter.Name.ToLower())));
            }

            return query;
        }
    }
}
