using Domain.Logging.Model;
using Domain.Logging.Port;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Infrastructure.Persistence.Repository.Logging;

public class EfLogRepository(ApplicationDbContext context) : GenericRepository<Log>(context), LogRepositoryPort
{
    public List<Log> GetLogsForReplay(string requestId)
    {
        var logs = context.Set<Log>().AsEnumerable();
        
        return logs.Where(l => 
            l.Metadata != null && 
            l.Metadata.TryGetValue("RequestId", out var id) && 
            id == requestId
        ).ToList();
    }

    public async Task<int> GetErrorCountForUserSince(Guid userId, DateTime since, CancellationToken cancellationToken = default)
    {
        return await context.Set<Log>()
            .CountAsync(x => 
                x.UserId == userId && 
                x.Level == "ERROR" && 
                x.CreatedAt >= since, 
                cancellationToken);
    }

    public async Task<double> GetErrorRateForUserSince(Guid userId, DateTime since, CancellationToken cancellationToken = default)
    {
        var totalLogs = await context.Set<Log>()
            .CountAsync(x => x.UserId == userId && x.CreatedAt >= since, cancellationToken);
        
        if (totalLogs == 0) return 0;

        var errorLogs = await context.Set<Log>()
            .CountAsync(x => 
                x.UserId == userId && 
                x.Level == "ERROR" && 
                x.CreatedAt >= since, 
                cancellationToken);

        return (double)errorLogs / totalLogs;
    }

    public async Task<double> GetAverageResponseTimeForUserSince(Guid userId, DateTime since, CancellationToken cancellationToken = default)
    {
        var avgResponseTime = await context.Set<Log>()
            .Where(x => 
                x.UserId == userId && 
                x.CreatedAt >= since &&
                x.Metadata != null &&
                x.Metadata.ContainsKey("ResponseTime"))
            .Select(x => double.Parse(x.Metadata["ResponseTime"]))
            .AverageAsync(cancellationToken);

        return avgResponseTime;
    }

    public async Task<int> GetApiErrorCountForUserSince(
        Guid userId, 
        DateTime since, 
        string? apiPath = null, 
        string? errorCode = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Set<Log>()
            .Where(x => 
                x.UserId == userId && 
                x.Level == "ERROR" &&
                x.CreatedAt >= since);

        if (!string.IsNullOrEmpty(apiPath))
        {
            query = query.Where(x => 
                x.Metadata != null && 
                x.Metadata.ContainsKey("ApiPath") && 
                x.Metadata["ApiPath"] == apiPath);
        }

        if (!string.IsNullOrEmpty(errorCode))
        {
            query = query.Where(x => 
                x.Metadata != null && 
                x.Metadata.ContainsKey("ErrorCode") && 
                x.Metadata["ErrorCode"] == errorCode);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<int> GetCustomEventCountForUserSince(
        Guid userId,
        DateTime since,
        string? eventName = null,
        string? eventValue = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Set<Log>()
            .Where(x => 
                x.UserId == userId && 
                x.CreatedAt >= since);

        if (!string.IsNullOrEmpty(eventName))
        {
            query = query.Where(x => 
                x.Metadata != null && 
                x.Metadata.ContainsKey("EventName") && 
                x.Metadata["EventName"] == eventName);
        }

        if (!string.IsNullOrEmpty(eventValue))
        {
            query = query.Where(x => 
                x.Metadata != null && 
                x.Metadata.ContainsKey("EventValue") && 
                x.Metadata["EventValue"] == eventValue);
        }

        return await query.CountAsync(cancellationToken);
    }
}

