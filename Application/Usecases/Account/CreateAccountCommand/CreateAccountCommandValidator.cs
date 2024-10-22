using Application.Common;
using Domain.Account.Port;
using FluentValidation;

namespace Application.Usecases.Account.CreateAccountCommand;

public class CreateAccountCommandValidator : BaseValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator(UserRepositoryPort port)
    {
        ValidateRequired(nameof(CreateAccountCommand.Email));
        ValidateRequired(nameof(CreateAccountCommand.Password));
        ValidateRequired(nameof(CreateAccountCommand.Username));

        RuleFor(x => x.Email)
            .MustAsync(async (email, cancellation) =>
            {
                var exist = await port!.ExistEmailByEmailAsync(email, cancellation);
                return !exist;
            })
            .WithMessage("User with this email already exists.")
            .WithErrorCode("EMAIL_ALREADY_EXISTS");

        RuleFor(x => x.Username)
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters long.")
            .WithErrorCode("USERNAME_TOO_SHORT")
            .MaximumLength(50)
            .WithMessage("Username must not exceed 50 characters.")
            .WithErrorCode("USERNAME_TOO_LONG");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("A valid email address is required.")
            .WithErrorCode("INVALID_EMAIL_FORMAT");

        RuleFor(x => x.Password)
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .WithErrorCode("PASSWORD_TOO_SHORT")
            .Matches(@"[A-Z]+")
            .WithMessage("Password must contain at least one uppercase letter.")
            .WithErrorCode("PASSWORD_MISSING_UPPERCASE")
            .Matches(@"[a-z]+")
            .WithMessage("Password must contain at least one lowercase letter.")
            .WithErrorCode("PASSWORD_MISSING_LOWERCASE")
            .Matches(@"[0-9]+")
            .WithMessage("Password must contain at least one number.")
            .WithErrorCode("PASSWORD_MISSING_NUMBER");
    }
}