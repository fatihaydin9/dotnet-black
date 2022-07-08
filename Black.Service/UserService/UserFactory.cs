using Black.Domain.Entity;
using Black.Domain.ValueObjects;
using Black.Model.User;

namespace Black.Service.UserService;

public sealed class UserFactory : IUserFactory
{
    public User Create(UserModel model, Auth auth)
    {
        return new User
        (
            new Name(model.FirstName, model.LastName),
            new Email(model.Email),
            auth
        );
    }
}
