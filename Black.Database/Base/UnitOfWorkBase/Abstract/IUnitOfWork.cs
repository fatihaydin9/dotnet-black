using Black.Database.Commands.Abstract;
using Black.Database.Queries.Abstract;
using Black.Database.Repositories.Abstract;

namespace Black.Infrastructure.UnitOfWorkBase.Abstract;

public interface IUnitOfWork : IAsyncDisposable
{
    IAuthRepository Auth { get; }   
    IUserRepository User { get; }   

    Task<int> SaveAsync();
}