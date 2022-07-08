using System.Linq.Expressions;

namespace Black.Database.Base.RepositoryBase.Abstract;

public interface IQueryRepository<T> where T : class, new()
{
    Task<T> FindByIdAsync(long id);
    Task<T> FindByGuidIdAsync(Guid guidId);
    Task<IQueryable<T>> FindAllAsync();
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
    Task<T> GetOnlyAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
}