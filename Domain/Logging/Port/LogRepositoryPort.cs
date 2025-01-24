using Domain.Common;
using Domain.Logging.Model;

namespace Domain.Logging.Port;

public interface LogRepositoryPort : IGenericRepository<Log>
{
    public List<Log> GetLogsForReplay(string requestId);
    public Task<int> GetErrorCountForUserSince(Guid userId, DateTime since, CancellationToken cancellationToken = default);
    public Task<double> GetErrorRateForUserSince(Guid userId, DateTime since, CancellationToken cancellationToken = default);
    public Task<double> GetAverageResponseTimeForUserSince(Guid userId, DateTime since, CancellationToken cancellationToken = default);
    public Task<int> GetApiErrorCountForUserSince(Guid userId, DateTime since, string? apiPath = null, string? errorCode = null, CancellationToken cancellationToken = default);
    public Task<int> GetCustomEventCountForUserSince(Guid userId, DateTime since, string? eventName = null, string? eventValue = null, CancellationToken cancellationToken = default);
}
