using System.Text;
using Integrations.Infrastructure.Http;
using Integrations.Sentry.Client.Contracts;
using Integrations.Sentry.Client.Models;
using Microsoft.Extensions.Logging;

namespace Integrations.Sentry.Client;

public class SentryHttpClient(HttpClient client, ILogger<SentryHttpClient> logger) : BaseHttpClient(client), ISentryHttpClient
{
    public async Task<SentryEventsResponse?> GetEventsAsync(string organisationName, string projectId, string environment, string dataset, bool disableAggregateExtrapolation, string field, string? statsPeriod = null, string? query = null)
    {
        var sb = new StringBuilder($"organizations/{organisationName}/events/");
        sb.Append($"?project={projectId}");
        sb.Append($"&environment={environment}");
        sb.Append($"&dataset={dataset}");
        sb.Append($"&disableAggregateExtrapolation={(disableAggregateExtrapolation ? "1" : "0")}");
        sb.Append($"&field={field}");
        if(!string.IsNullOrEmpty(statsPeriod)) sb.Append($"&statsPeriod={statsPeriod}");
        if(!string.IsNullOrEmpty(query)) sb.Append($"&query={query}");

        var url = sb.ToString();

        var httpRequest = new HttpRequestMessage
        {
            RequestUri = new Uri(url, UriKind.Relative),
            Method = HttpMethod.Get
        };
        
        logger.LogInformation("Sending request to Sentry API: {Method} {Url}", httpRequest.Method, httpRequest.RequestUri);
        
        try
        {
            var response = await SendAsync<SentryEventsResponse>(httpRequest);
            logger.LogInformation("Successfully received response from Sentry API");
            
            return response;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Failed to retrieve events from Sentry API");
            throw;
        }
    }
}