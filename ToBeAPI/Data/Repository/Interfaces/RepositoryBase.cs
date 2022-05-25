using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ToBeApi.Data.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T :class
    {

        protected ApplicationDBContext ApplicationDBContext;

        public RepositoryBase(ApplicationDBContext applicationDBContext)
        {
            ApplicationDBContext = applicationDBContext;
        }

        public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ?
            ApplicationDBContext.Set<T>().AsNoTracking() : 
            ApplicationDBContext.Set<T>();
    
        public IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression,bool trackChanges) => !trackChanges ? 
            ApplicationDBContext.Set<T>().Where(expression).AsNoTracking() :
            ApplicationDBContext.Set<T>().Where(expression);

        public void Create(T entity) => ApplicationDBContext.Set<T>().Add(entity);

        public void Update(T entity) => ApplicationDBContext.Set<T>().Update(entity);

        public void Delete(T entity) => ApplicationDBContext.Set<T>().Remove(entity);

        public int Count() => ApplicationDBContext.Set<T>().Count();


    }
}
