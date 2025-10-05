using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Weather.Data.Interface;

namespace Weather.Data.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly DataBaseContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }

        // ✅ Async Methods
        public async Task InsertAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return predicate == null
                ? await _dbSet.ToListAsync()
                : await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IQueryable<T>> GetAllQueryableAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(
                predicate == null ? _dbSet.AsQueryable() : _dbSet.Where(predicate).AsQueryable()
            );
        }

        // ✅ Sync Methods
        public List<T> GetAll(Func<T, bool> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            foreach (var property in includeProperties)
                query = query.Include(property);

            return predicate == null ? query.ToList() : query.Where(predicate).ToList();
        }

        public IEnumerable<T> GetAllByQuery(Func<T, bool> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            foreach (var property in includeProperties)
                query = query.Include(property);

            return predicate == null ? query : query.Where(predicate).AsQueryable();
        }

        public IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            foreach (var property in includeProperties)
                query = query.Include(property);

            return predicate == null ? query : query.Where(predicate);
        }

        public T GetById(long id) => _dbSet.Find(id);

        public void Insert(T entity) => _dbSet.Add(entity);
        public void InsertRange(List<T> entities) => _dbSet.AddRange(entities);

        public void Update(T entity) => _dbSet.Update(entity);
        public void UpdateRange(List<T> entities) => _dbSet.UpdateRange(entities);

        public void Update(T entity, T old)
        {
            if (entity == null || old == null) return;
            _dbContext.Entry(old).CurrentValues.SetValues(entity);
        }

        public void Delete(T entity) => _dbSet.Remove(entity);

        public void Delete(long id)
        {
            T entity = _dbSet.Find(id);
            if (entity != null)
                Delete(entity);
        }
    }
}
