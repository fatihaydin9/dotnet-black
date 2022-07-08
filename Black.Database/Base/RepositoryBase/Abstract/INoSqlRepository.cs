namespace Black.Database.Base.RepositoryBase.Abstract;

public interface INoSqlRepository<T> where T : class, new()
{
    Task<T> GetByGuidIdNoSql(Guid guidId);
    Task<T> GetByIdNoSql(long id);
    Task<IList<T>> GetAllAsyncNoSql();
    Task CreateAsyncNoSql(T entity);
    Task UpdateAsyncNoSql(T entity);
    Task RemoveAsyncNoSql(Guid guidId);
    Task RemoveAsyncNoSql(long id);
}
