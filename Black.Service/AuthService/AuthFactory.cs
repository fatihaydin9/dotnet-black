using Black.Domain.Entity;
using Black.Domain.Enumeration;
using Black.Model.Authentication;

namespace Black.Service.AuthService;

public sealed class AuthFactory : IAuthFactory
{
    public Auth Create(AuthModel model) => new Auth(model.Login, model.Password, (Roles)model.Roles);
}