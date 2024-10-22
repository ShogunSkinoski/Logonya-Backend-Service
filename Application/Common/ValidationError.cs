using FluentResults;

namespace Application.Common;

public class ValidationError: Error
{
   public string ErrorCode { get; }

    public ValidationError(string message, string errorCode)
        : base(message)
    {

        Metadata.Add("ErrorCode", errorCode);

    }
}