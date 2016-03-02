﻿namespace ProcessingTools.Data.Common.Elasticsearch.Tests.Fakes
{
    using System;
    using Nest;
    using ProcessingTools.Data.Common.Elasticsearch.Contracts;

    public class FakeElasticContextProvider : IElasticContextProvider
    {
        public IndexName Context
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
    }
}
