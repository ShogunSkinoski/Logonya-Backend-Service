using Domain.Logging.Model;

namespace Application.Usecases.Logging.CreateLogCommand;

public sealed record CreateLogMessage
{
    public Guid Id { get; set; }
    public string Action { get; set; }
    public string Description { get; set; }
    public string Level { get; set; }
    public string Source { get; set; }
    public string Environment { get; set; }
    public string IpAddress { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
    public string UserName { get; set; }
    public Guid ApiKeyId { get; set; }
    public Guid? UserId { get; set; }
    public string? Exception { get; set; }
    public string? StackTrace { get; set; }
    public DateTime CreatedAt { get; set; }

    public static CreateLogMessage FromLog(Log log)
    {
        return new CreateLogMessage
        {
            Id = log.Id,
            Action = log.Action,
            Description = log.Description,
            Level = log.Level,
            Source = log.Source,
            Environment = log.Environment,
            IpAddress = log.IpAddress,
            Metadata = log.Metadata,
            UserName = log.UserName,
            ApiKeyId = (Guid)log.ApiKeyId,
            UserId = log.UserId,
            Exception = log.Exception,
            StackTrace = log.StackTrace,
            CreatedAt = log.CreatedAt
        };
    }
}
