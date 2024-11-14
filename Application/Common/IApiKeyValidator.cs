namespace Application.Common;

public interface IApiKeyValidator
{
    Task<bool> IsValidApiKey(string apiKey, CancellationToken cancellationToken = default);
}
