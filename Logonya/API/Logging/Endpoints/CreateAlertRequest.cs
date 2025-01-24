using Application.Usecases.Logging.CreateAlertCommand;

namespace Presentation.API.Logging;

public sealed record CreateAlertRequest(
    string Name,
    string Description,
    AlertCondition Condition,
    string Channel,
    string Target
); 