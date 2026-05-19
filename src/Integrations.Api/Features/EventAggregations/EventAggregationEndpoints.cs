using Integrations.Core.Contracts;
using Integrations.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class EventAggregationEndpoints
{
    public static IEndpointRouteBuilder MapEventAggregationEndpoints(this IEndpointRouteBuilder app)
    {
        var apiGroup = app
            .MapGroup("/api/v1/eventaggregations")
            .WithTags("Event Aggregations");

        apiGroup
            .MapGet("", Get)
            .WithName("GetEventAggregations");

        return app;
    }

    private static async Task<Results<Ok<IEnumerable<EventAggregation>>, InternalServerError>> Get(IEventAggregationRepository<EventAggregation> repository)
    {
        var eventAggregations = await repository.GetAllAsync();

        return TypedResults.Ok(eventAggregations.Select(x => new EventAggregation
        {
            Id = x.Id,
            EventType = x.EventType,
            EventPeriod = x.EventPeriod,
            Count = x.Count,
            CreatedAt = x.CreatedAt,
            LastModifiedAt = x.LastModifiedAt,
            CreatedBy = x.CreatedBy,
            LastModifiedBy = x.LastModifiedBy
        }));
    }
}