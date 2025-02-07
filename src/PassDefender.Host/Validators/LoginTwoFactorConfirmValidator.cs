using FluentValidation;
using PassDefender.Host.Requests.Login;

namespace PassDefender.Host.Validators;

public class LoginTwoFactorConfirmValidator : AbstractValidator<LoginTwoFactorConfirmRequest>
{
    public LoginTwoFactorConfirmValidator()
    {
        RuleFor(request => request.Code)
            .NotNull()
            .NotEmpty();
        
        RuleFor(request => request.Email)
            .NotNull()
            .NotEmpty();
    }
}