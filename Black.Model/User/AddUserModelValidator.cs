namespace Black.Model.User;

public sealed class AddUserModelValidator : UserModelValidator
{
    public AddUserModelValidator()
    {
        FirstName(); LastName(); Email(); Auth();
    }
}
