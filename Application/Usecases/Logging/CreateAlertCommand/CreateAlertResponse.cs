namespace Application.Usecases.Logging.CreateAlertCommand
{
    public sealed record CreateAlertResponse(
        Guid AlertId,
        string Name,
        DateTime CreatedAt
    );
} 