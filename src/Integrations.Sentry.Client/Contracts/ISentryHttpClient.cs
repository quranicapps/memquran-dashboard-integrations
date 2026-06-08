using Integrations.Sentry.Client.Models;

namespace Integrations.Sentry.Client.Contracts;

public interface ISentryHttpClient
{
    Task<SentryEventsResponse?> GetEventsAsync(string organisationName, string projectId, string environment, string dataset, bool disableAggregateExtrapolation, string field, string? statsPeriod = null, string? query = null);
}