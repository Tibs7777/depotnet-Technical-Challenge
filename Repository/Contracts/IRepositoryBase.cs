using System.Linq.Expressions;

namespace Repository.Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool asTracking);

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool asTracking);

        void Create(T entity);

        void CreateRange(IEnumerable<T> entities);

        void Update(T entity);

        void Delete(T entity);
    }
}
