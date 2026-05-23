using Microsoft.AspNetCore.Http;

namespace Integrations.Sentry.Functions.Models;

/// <summary>
/// Map requests from Query String and from Request Body
/// </summary>
public class EventAggregationRequest
{
    public string? Dataset { get; init; }
    public string? StatsPeriod { get; init; }

    public static EventAggregationRequest MapFromQueryCollection(IQueryCollection queryCollection)
    {
        return new EventAggregationRequest
        {
            Dataset = queryCollection["dataset"],
            StatsPeriod = queryCollection["statsPeriod"]
        };
    }
    
    public static async Task<EventAggregationRequest?> MapFromHttpRequest(HttpRequest httpRequest)
    {
        return await httpRequest.ReadFromJsonAsync<EventAggregationRequest>();
    }
    
    public override string ToString()
    {
        return $"Dataset: {Dataset}, StatsPeriod: {StatsPeriod}";
    }
}