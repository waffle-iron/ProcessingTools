﻿namespace ProcessingTools.Data.Common.Repositories.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRepository<T>
    {
        Task<IQueryable<T>> All();

        Task<T> GetById(object id);

        Task Add(T entity);

        Task Update(T entity);

        Task Delete(T entity);

        Task Delete(object id);

        Task<int> SaveChanges();
    }
}
