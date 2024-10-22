using FluentResults;
using FluentValidation.Results;
using System.Linq;

namespace Application.Common;

public static class ValidationResultExtensions
{
    public static Result<T> ToResult<T>(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors.Select(error =>
                new ValidationError(error.ErrorMessage, error.ErrorCode)
            );

        return Result.Fail<T>(errors);
    }
}
