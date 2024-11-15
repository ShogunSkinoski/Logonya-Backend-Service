namespace Application.Usecases.Logging.CreateLogCommand;

public sealed record CreateLogResponse
{
    public Guid Id { get; init; }
    public string Action { get; init; }
    public string Level { get; init; }
    public string Source { get; init; }
    public DateTime CreatedAt { get; init; }

    public CreateLogResponse(Guid id, string action, string level, string source, DateTime createdAt)
    {
        Id = id;
        Action = action;
        Level = level;
        Source = source;
        CreatedAt = createdAt;
    }
}
