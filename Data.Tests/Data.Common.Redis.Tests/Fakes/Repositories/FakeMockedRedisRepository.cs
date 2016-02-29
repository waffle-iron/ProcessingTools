﻿namespace ProcessingTools.Data.Common.Redis.Tests.Fakes.Repositories
{
    using Mocks;
    using Models;
    using Models.Contracts;

    using ProcessingTools.Data.Common.Redis.Contracts;
    using ProcessingTools.Data.Common.Redis.Repositories;

    public class FakeMockedRedisRepository : RedisGenericRepository<SimpleTimeRecordModel>
    {
        public FakeMockedRedisRepository()
            : base(new RedisClientProviderMock())
        {
        }
    }
}