using Black.Database.Base.RepositoryBase.Abstract;
using Black.Infrastructure.Objects;
using Black.Model.User;
using DotNetCore.Objects;

namespace Black.Database.Queries.Abstract;

public interface IUserQuery
{
    Task<List<UserModel>> FindAllAsync();
    Task<UserModel> FindByIdAsync(long id);
    Task<UserModel> FindByGuidIdAsync(Guid guidId);
    Task<long> GetAuthIdByUserIdAsync(long id);
    Task<Grid<UserModel>> GridAsync(GridParameters parameters);
    Task<List<SelectBox>> GetSelectBoxList();
}
