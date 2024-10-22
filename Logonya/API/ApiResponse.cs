using FluentResults;

namespace Presentation.API;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public List<ApiError> Errors { get; set; }

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
}

public class ApiError
{
    public string Message { get; set; }
    public string ErrorCode { get; set; }

}
