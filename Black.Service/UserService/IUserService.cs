using Black.Model.User;
using DotNetCore.Objects;
using DotNetCore.Results;

namespace Black.Service.UserService;

public interface IUserService
{
    Task<IResult<long>> AddAsync(UserModel model);
    Task<IResult> DeleteAsync(long id);
    Task<UserModel> GetAsync(long id);
    Task<Grid<UserModel>> GridAsync(GridParameters parameters);
    Task<IResult> PassiveAsync(long id);
    Task<IEnumerable<UserModel>> ListAsync();
    Task<IResult> UpdateAsync(UserModel model);
}

