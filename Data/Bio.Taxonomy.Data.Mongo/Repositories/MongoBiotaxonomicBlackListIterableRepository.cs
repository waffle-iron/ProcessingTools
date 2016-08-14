﻿namespace ProcessingTools.Bio.Taxonomy.Data.Mongo.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Contracts;
    using Models;

    using MongoDB.Driver;
    using ProcessingTools.Bio.Taxonomy.Data.Common.Models.Contracts;
    using ProcessingTools.Data.Common.Mongo.Contracts;
    using ProcessingTools.Data.Common.Mongo.Repositories;

    public class MongoBiotaxonomicBlackListIterableRepository : MongoRepository<MongoBlackListEntity>, IMongoBiotaxonomicBlackListIterableRepository
    {
        public MongoBiotaxonomicBlackListIterableRepository(IMongoDatabaseProvider provider)
            : base(provider)
        {
        }

        public Task<IQueryable<IBlackListEntity>> All() => Task.Run(() =>
        {
            return this.Collection
                .AsQueryable()
                .Cast<IBlackListEntity>();
        });
    }
}
