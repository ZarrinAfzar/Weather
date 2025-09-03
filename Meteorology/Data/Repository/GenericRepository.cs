using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Weather.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace Weather.Data.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {

        private readonly DbContext _dbContext = null;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }




        #region Async
 
        //public T GetByIdasync(int id)
        //{
        //    return _dbSet.Find(id);
        //}

        //public void Insertasync(T entity)
        //{
        //    _dbSet.Add(entity);
        //}

        //public void Updateasync(T entity)
        //{
        //    _dbSet.Update(entity);
        //}

        //public void Updateasync(T entity, T old)
        //{
        //    _dbContext.Entry(old).CurrentValues.SetValues(entity);
        //}

        //public void Deleteasync(T entity)
        //{
        //    _dbSet.Remove(entity);
        //}

        //public void Deleteasync(int id)
        //{
        //    T entityToDelete = _dbSet.Find(id);
        //    Delete(entityToDelete);
        //}
        #endregion


        #region sync
        public List<T> GetAll(Func<T, bool> predicate = null,  params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }
            if (predicate != null)
            {
                query = query.Where(predicate).AsQueryable();
            }

            return query.ToList();
        }
        //public IQueryable<T> GetAllByQuery(Func<T, bool> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        //{
        //    IQueryable<T> query = _dbSet;
        //    if (includeProperties != null)
        //    {
        //        foreach (var property in includeProperties)
        //        {
        //            query = query.Include(property);
        //        }
        //    }
        //    if (predicate != null)
        //    {
        //        query = query.Where(predicate).AsQueryable();
        //    }
        //    return query;
        //}

        public IEnumerable<T> GetAllByQuery(Func<T, bool> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            query.AsNoTracking();
            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }
            }
            if (predicate != null)
            {
                query = query.Where(predicate).AsQueryable();
            }
            return query; 
        }



        public T GetById(long id)
        {
            return _dbSet.Find(id);
        } 


        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }
        public void InsertRange(List<T> entitys) 
        {
            _dbSet.AddRange(entitys); 
        }


        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void UpdateRange(List<T> entity)
        {
            _dbSet.UpdateRange(entity);
        }
        public void Update(T entity, T old)
        {
            _dbContext.Entry(old).CurrentValues.SetValues(entity);
        }
        public void UpdateRange(List<T> entity, List<T> old)
        {
            _dbContext.Entry(old).CurrentValues.SetValues(entity);
        }


        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        public void Delete(long id)
        {
            T entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }



        #endregion

    }

}
