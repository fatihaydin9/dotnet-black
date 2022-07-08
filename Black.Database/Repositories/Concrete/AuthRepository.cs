using Black.Database.Repositories.Abstract;
using Black.Database.RepositoryBase.Concrete;
using Black.Domain;
using Black.Domain.Entity;
using Black.Infrastructure.UnitOfWorkBase.Concrete;

namespace Black.Database.Repositories.Concrete;

public class AuthRepository : GenericRepository<Auth>
{
    public AuthRepository(RelationalDbContext context) : base(context)
    {

    }
}