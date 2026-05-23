using Integrations.Core;
using Integrations.Sentry.Client.Contracts;
using Integrations.Sentry.Functions.Contracts;
using Integrations.Sentry.Functions.Settings;

namespace Integrations.Sentry.Functions.Services;

public class SentryService(ISentryHttpClient sentryHttpClient, SentrySettings sentrySettings) : ISentryService
{
    public async Task<Result<long>> GetSentryEventAggregationSpanCountAsync(string? statsPeriod = null, string? query = null)
    {
        var response = await sentryHttpClient.GetEventsAsync(sentrySettings.OrganizationName, sentrySettings.ProjectId, sentrySettings.Environment, "spans", true, "count(span.duration)", statsPeriod, query);
        if(response?.Data == null || response.Data.Count == 0)
        {
            return Result<long>.Failure("No data returned from Sentry API in the \"data\" property.");
        }
        
        var count = response.Data.Select(x => x.CountSpanDuration).FirstOrDefault();

        return count == null
            ? Result<long>.Failure("No count returned from Sentry API in the \"count(span.duration)\" field.") 
            : Result<long>.SuccessObjectResult((long)count);
    }
}