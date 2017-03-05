﻿namespace ProcessingTools.Contracts.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IFilterableRepository<T> : IRepository<T>
    {
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> filter);
    }
}
