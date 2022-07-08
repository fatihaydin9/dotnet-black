using Black.Domain.Entity;
using Black.Model.User;

namespace Black.Service.UserService;

public interface IUserFactory
{
    User Create(UserModel model, Auth auth);
}
