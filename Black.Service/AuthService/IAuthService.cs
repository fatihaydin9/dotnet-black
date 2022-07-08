using Black.Domain.Entity;
using Black.Model.Authentication;
using DotNetCore.Results;

namespace Black.Service.AuthService;

public interface IAuthService
{
    Task<IResult<Auth>> AddAsync(AuthModel model);
    Task DeleteAsync(long id);
    Task<IResult<TokenModel>> SignInAsync(SignInModel model);
}
