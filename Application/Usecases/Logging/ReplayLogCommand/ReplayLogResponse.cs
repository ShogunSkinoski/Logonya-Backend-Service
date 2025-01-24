using Domain.Logging.Model;

namespace Application.Usecases.Logging.ReplayLogCommand;

public sealed record ReplayLogResponse
{
    public List<Log> Logs { get; init; }
    public string Analysis { get; init; }

    public ReplayLogResponse(List<Log> logs, string analysis)
    {
        Logs = logs;
        Analysis = analysis;
    }
}