namespace Black.Database.Base.RepositoryBase.Abstract;

public interface IGenericRepository<T> : ICommandRepository<T>, IQueryRepository<T> where T : class, new()
{

}
