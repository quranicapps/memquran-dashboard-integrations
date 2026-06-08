namespace Integrations.Sentry.Functions.Settings;

public class SentrySettings
{
    public static string SectionName  => "SentrySettings";

    public string Environment { get; init; } = null!;
    public string BaseUrl { get; init; } = null!;
    public string ApiKey { get; init; } = null!;
    public string OrganizationName { get; init; } = null!;
    public string ProjectId { get; init; } = null!;
    public string Type { get; set; } = null!;
    public string[] EventAggregationPeriods { get; init; } = [];
}