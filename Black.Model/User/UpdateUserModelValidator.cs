namespace Black.Model.User;

public sealed class UpdateUserModelValidator : UserModelValidator
{
    public UpdateUserModelValidator()
    {
        Id(); FirstName(); LastName(); Email();
    }
}
