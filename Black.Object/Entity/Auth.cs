using Black.Domain.Base;
using Black.Domain.Enumeration;

namespace Black.Domain.Entity;

public class Auth : EntityBase
{
    public Auth()
    {

    }

    public Auth
    (
        string login,
        string password,
        Roles roles
    )
    {
        Login = login;
        Password = password;
        Roles = roles;
        GuiddId = Guid.NewGuid().ToString();
    }

    public string Login { get; private set; }

    public string Password { get; private set; }

    public string GuiddId { get; private set; }

    public Roles Roles { get; private set; }

    public void UpdatePassword(string password)
    {
        Password = password;
    }
}