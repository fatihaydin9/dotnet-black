using Black.Database.RepositoryBase.Concrete;
using Black.Domain.Entity;
using Black.Infrastructure.UnitOfWorkBase.Concrete;

namespace Black.Database.Repositories.Concrete;

public class UserRepository : GenericRepository<User>
{
    public UserRepository(RelationalDbContext context) : base(context)
    {

    }
}