using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using System.Linq.Expressions;

namespace Repository
{
    internal abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll(bool asTracking = true)
        {
            return asTracking ? RepositoryContext.Set<T>() : RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool asTracking = true)
        {
            var query = RepositoryContext.Set<T>().Where(expression);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public void Create(T entity)
        {
            RepositoryContext.Set<T>().Add(entity);
        }

        public void CreateRange(IEnumerable<T> entities)
        {
            RepositoryContext.Set<T>().AddRange(entities);
        }

        public void Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
        }
    }
}