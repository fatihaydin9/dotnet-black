using FluentValidation;

namespace Black.Model.Authentication;

public sealed class SignInModelValidator : AbstractValidator<SignInModel>
{
    public SignInModelValidator()
    {
        RuleFor(signIn => signIn.Login).NotEmpty();
        RuleFor(signIn => signIn.Password).NotEmpty();
    }
}