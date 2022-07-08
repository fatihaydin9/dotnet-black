using Black.Database.Repositories.Abstract;
using Black.Database.Repositories.Concrete;
using Black.Infrastructure.UnitOfWorkBase.Abstract;

namespace Black.Infrastructure.UnitOfWorkBase.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly RelationalDbContext _context;

    private UserRepository _userRepository;
    private AuthRepository _authRepository;
    public UnitOfWork(RelationalDbContext context)
    {
        _context = context;
    }

    public IUserRepository User => (IUserRepository)(_userRepository ?? new UserRepository(_context));
    public IAuthRepository Auth => (IAuthRepository)(_authRepository ?? new AuthRepository(_context));


    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}