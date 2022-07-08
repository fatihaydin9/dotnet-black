using Black.Domain.Entity;
using Black.Model.Authentication;

namespace Black.Service.AuthService;

public interface IAuthFactory
{
    Auth Create(AuthModel model);
}
