using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Weather.Data.Interface
{
    public interface IRepository<T> where T : class
    {
        // ✅ Async
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> GetAllQueryableAsync(Expression<Func<T, bool>> predicate);

        // ✅ Sync
        List<T> GetAll(Func<T, bool> predicate = null, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAllByQuery(Func<T, bool> predicate = null, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
        T GetById(long id);

        void Insert(T entity);
        void InsertRange(List<T> entities);

        void Update(T entity);
        void UpdateRange(List<T> entities);
        void Update(T entity, T old);

        void Delete(T entity);
        void Delete(long id);
    }
}
