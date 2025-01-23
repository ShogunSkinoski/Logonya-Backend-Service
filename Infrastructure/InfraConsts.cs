namespace Infrastructure;

public static class InfraConsts
{
    public static class KafkaTopicNames
    {
        public const string LOG_VECTORIZATION = "logging.vectorization";
        public const string WEBHOOK_NOTIFICATIONS = "webhook.notifications";
    }

    public static class WebhookEvents
    {
        public const string LOG_CREATED = "log.created";
        public const string ERROR_DETECTED = "error.detected";
        public const string RATE_LIMIT_EXCEEDED = "rate_limit.exceeded";
        public const string API_KEY_EXPIRED = "api_key.expired";
    }

    public static class KafkaTopicKeys
    {
        public const string LOG_VECTORIZATION = "LogVectorization";
        public const string WEBHOOK_NOTIFICATIONS = "WebhookNotifications";
    }
}
