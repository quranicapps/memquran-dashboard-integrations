using Integrations.Core;

namespace Integrations.Sentry.Functions.Contracts;

public interface ISentryService
{
    Task<Result<long>> GetSentryEventAggregationSpanCountAsync(string? statsPeriod = null, string? query = null);
}