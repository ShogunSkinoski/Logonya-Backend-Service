using Domain.Account.Model;
using System.Text.Json;

namespace Domain.Logging.Model;

public class Log
{
    public Guid Id { get; private set; }
    public string Action { get; private set; }
    public string Description { get; private set; }
    public string Level { get; private set; }
    public string Source { get; private set; }
    public string Environment { get; private set; }
    private JsonDocument? _metadata;
    private Dictionary<string, string>? _metadataCache;

    public Dictionary<string, string> Metadata
    {
        get
        {
            if (_metadataCache == null && _metadata != null)
            {
                _metadataCache = JsonSerializer.Deserialize<Dictionary<string, string>>(_metadata);
            }
            return _metadataCache ?? new Dictionary<string, string>();
        }
        private set
        {
            _metadataCache = value;
            _metadata = value != null
                ? JsonDocument.Parse(JsonSerializer.Serialize(value))
                : null;
        }
    }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public string? UserName { get; private set; }
    public string IpAddress { get; private set; }
    public Guid? ApiKeyId { get; private set; }
    public ApiKey? ApiKey { get; private set; }
    public string? Exception { get; private set; }
    public string? StackTrace { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Log(
        string action,
        string description,
        string level,
        string source,
        string environment,
        string ipAddress,
        Guid userId,
        Dictionary<string, string>? metadata = null,
        string? userName = null,
        Guid? apiKeyId = null,
        string? exception = null,
        string? stackTrace = null)
    {
        Id = Guid.NewGuid();
        Action = action;
        Description = description;
        Level = level;
        Source = source;
        Environment = environment;
        IpAddress = ipAddress;
        Metadata = metadata ?? new Dictionary<string, string>();
        UserId = userId;
        UserName = userName;
        ApiKeyId = apiKeyId;
        Exception = exception;
        StackTrace = stackTrace;
        CreatedAt = DateTime.UtcNow;
    }
}
