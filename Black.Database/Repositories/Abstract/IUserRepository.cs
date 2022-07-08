using Black.Database.Base.RepositoryBase.Abstract;
using Black.Domain.Entity;
using Black.Model.User;
using DotNetCore.Objects;

namespace Black.Database.Repositories.Abstract;

public interface IUserRepository : IGenericRepository<User>
{
    Task<long> GetAuthIdByUserIdAsync(long id);
    Task<UserModel> GetModelAsync(long id);
    Task<Grid<UserModel>> GridAsync(GridParameters parameters);
    Task<IEnumerable<UserModel>> ListModelAsync();
    Task UpdateStatusAsync(User user);
}