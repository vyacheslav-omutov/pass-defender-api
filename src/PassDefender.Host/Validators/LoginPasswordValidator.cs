using FluentValidation;
using PassDefender.Host.Requests.Login;

namespace PassDefender.Host.Validators;

public class LoginPasswordValidator : AbstractValidator<LoginPasswordRequest>
{
    public LoginPasswordValidator()
    {
        RuleFor(request => request.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();

        RuleFor(request => request.Password)
            .NotEmpty()
            .NotNull();
    }
}