using System.Text.Json.Serialization;

namespace Application.Usecases.Logging.CreateAlertCommand
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MetricType
    {
        ERROR_COUNT,
        ERROR_RATE,
        RESPONSE_TIME,
        API_ERRORS,
        CUSTOM_EVENT
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OperatorType
    {
        GREATER_THAN,
        LESS_THAN,
        EQUALS,
        GREATER_THAN_OR_EQUALS,
        LESS_THAN_OR_EQUALS
    }

    public class AlertCondition
    {
        public MetricType MetricType { get; init; }
        public OperatorType Operator { get; init; }
        public double Threshold { get; init; }
        public int TimeWindowMinutes { get; init; }
        public Dictionary<string, string>? CustomParameters { get; init; }
    }
} 