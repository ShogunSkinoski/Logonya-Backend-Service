using FluentResults;
using System.Text.RegularExpressions;

namespace Presentation.API;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public List<ApiError>? Errors { get; set; }

    public static ApiResponse<T> FromResult(Result<T> result)
    {
        return new ApiResponse<T>
        {
            IsSuccess = result.IsSuccess,
            Data = result.IsSuccess ? result.Value : default,
            Errors = result.IsFailed ? result.Errors.Select(e => new ApiError
            {
                Message = e.Message,
                ErrorCode = e.Metadata.ContainsKey("ErrorCode") ? e.Metadata["ErrorCode"].ToString() : null,
            }).ToList() : null
        };
    }
    public static ApiResponse<T> FromException(Exception exception)
    {
        if (exception == null) return null;
        var errorRegex = new Regex(@"ValidationError with Message='([^']*)', Metadata='\[ErrorCode, ([^\]]*)\]'");
        var matches = errorRegex.Matches(exception.Message);
        var errors = matches.Select(match => new ApiError
        {
            ErrorCode = match.Groups[1].Value,
            Message = match.Groups[2].Value,
        });

        return new ApiResponse<T>
        {
            Errors = errors.ToList(),
        };
    }
}

public class ApiError
{
    public string Message { get; set; }
    public string ErrorCode { get; set; }

}
