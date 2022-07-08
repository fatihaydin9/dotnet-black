namespace Black.Database.Base.RepositoryBase.Abstract;

public interface ICommandRepository<T> where T : class, new()
{
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(long id);
    Task DeleteAsync(Guid guidId);
    Task AddRangeAsync(IEnumerable<T> entities);
    void UpdateRange(IEnumerable<T> entities);
    void DeleteRange(IEnumerable<T> entities);
}
