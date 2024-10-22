using FluentValidation;

namespace Application.Common;

public abstract class BaseValidator<TEntity> : AbstractValidator<TEntity>
{
    protected IRuleBuilderOptions<TEntity, object> ValidateRequired(string propertyName)
    {
        return RuleFor(x => x.GetType().GetProperty(propertyName).GetValue(x, null))
                    .NotEmpty()
                    .WithMessage($"{propertyName} is required")
                    .WithErrorCode($"{propertyName.ToUpper()}_REQUIRED");
    }
}