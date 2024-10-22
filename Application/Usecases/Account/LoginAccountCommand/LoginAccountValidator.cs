using Application.Common;
using Domain.Account.Port;
using FluentValidation;

namespace Application.Usecases.Account.LoginAccountCommand;

public class LoginAccountValidator : BaseValidator<LoginAccountCommand>
{
    public LoginAccountValidator(UserRepositoryPort port)
    {
        ValidateRequired(nameof(LoginAccountCommand.Email));
        ValidateRequired(nameof(LoginAccountCommand.Password));
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .WithErrorCode("INVALID_EMAIL_FORMAT");

        RuleFor(x => x.Password)
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .WithErrorCode("PASSWORD_TOO_SHORT");
    }
}
