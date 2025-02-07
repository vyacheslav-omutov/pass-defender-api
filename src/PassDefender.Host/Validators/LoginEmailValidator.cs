using FluentValidation;
using PassDefender.Host.Requests.Login;

namespace PassDefender.Host.Validators;

public class LoginEmailValidator : AbstractValidator<LoginTwoFactorRequest>
{
    public LoginEmailValidator()
    {
        RuleFor(request => request.Email)
            .NotNull()
            .NotEmpty();
    }
}