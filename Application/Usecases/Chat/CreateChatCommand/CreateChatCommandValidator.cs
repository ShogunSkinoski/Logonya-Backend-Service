using Application.Common;
using FluentValidation;

namespace Application.Usecases.Chat.CreateChatCommand;

public class CreateChatCommandValidator : BaseValidator<CreateChatCommand>
{
    public CreateChatCommandValidator()
    {
        ValidateRequired(nameof(CreateChatCommand.UserId));
        ValidateRequired(nameof(CreateChatCommand.Title));

        RuleFor(x => x.Title)
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters")
            .WithErrorCode("TITLE_TOO_LONG");
    }
} 