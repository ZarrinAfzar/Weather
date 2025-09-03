using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Weather.Data.Interface
{
    public interface IRepository<T> where T : class
    {


        #region Async
        //Task<IEnumerable<T>> GetAllasync(Func<T, bool> predicate = null);
        //Task<T> GetByIdasync(int id);

        //Task Insertasync(T entity);

        //Task Updateasync(T entity);
        //Task Updateasync(T entity, T old);

        //Task Deleteasync(T entity);
        //Task Deleteasync(int entity);
        #endregion


        #region Sync 
        List<T> GetAll(Func<T, bool> predicate = null,   params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAllByQuery(Func<T, bool> predicate = null,   params Expression<Func<T, object>>[] includeProperties);
        //IEnumerable<T> GetAll(Func<T, bool> predicate = null, Expression<Func<T, object>> orderPredicate);
        //IEnumerable<T> GetAll(Func<T, bool> predicate = null, params Expression<Func<T, object>>[] includeProperties);
        //IEnumerable<T> GetAll(Func<T, bool> predicate = null, Expression<Func<T, object>> orderPredicate, params Expression<Func<T, object>>[] includeProperties);
        T GetById(long id); 

        void Insert(T entity);
        void InsertRange(List<T> entitys); 

        void Update(T entity);
        void UpdateRange(List<T> entity);
        void Update(T entity, T old);
        void UpdateRange(List<T> entity, List<T> old);

        void Delete(T entity);
        void Delete(long entity);
        #endregion


    }
}
