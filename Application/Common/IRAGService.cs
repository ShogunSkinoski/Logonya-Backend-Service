namespace Application.Common;

public interface IRAGService
{
    Task<string> SearchAsync(string query, int limit = 10, CancellationToken cancellationToken = default);
}
