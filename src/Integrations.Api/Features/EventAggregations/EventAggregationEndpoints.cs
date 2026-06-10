using Integrations.Api.Settings;
using Integrations.Core;
using Integrations.Core.Contracts;
using Integrations.Core.Models;
using Integrations.Sentry.Client.Contracts;
using Integrations.Sentry.Client.Models;
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
            .MapGet("", GetAllEventAggregationsAsync)
            .WithName("GetAllEventAggregations");
        
        apiGroup
            .MapGet("/{eventType:int}/{eventPeriod:int}", GetEventAggregationsByTypeAndPeriodAsync)
            .WithName("GetEventAggregationsByTypeAndPeriod");
        
        apiGroup
            .MapGet("sentry/{dataset}/{statsPeriod}", GetSentryEventAggregationsAsync)
            .WithName("GetSentryEventAggregations");
        
        apiGroup
            .MapGet("TooManyRequests", BadSentryEventAggregationsAsync)
            .WithName("Test");

        return app;
    }

    private static async Task<Results<Ok<ApiResponse<IEnumerable<EventAggregation>>>, InternalServerError>> GetAllEventAggregationsAsync(IEventAggregationRepository<EventAggregation> repository)
    {
        var eventAggregations = await repository.GetAllAsync(EventType.All, EventPeriod.Last30Days);

        var aggregations = eventAggregations.Select(x => new EventAggregation
        {
            Id = x.Id,
            EventType = x.EventType,
            EventPeriod = x.EventPeriod,
            Count = x.Count,
            CreatedAt = x.CreatedAt,
            LastModifiedAt = x.LastModifiedAt,
            CreatedBy = x.CreatedBy,
            LastModifiedBy = x.LastModifiedBy
        }).ToList();
        
        return TypedResults.Ok(new ApiResponse<IEnumerable<EventAggregation>>(aggregations, Meta: new { aggregations.Count}));
    }

    private static async Task<Results<Ok<ApiResponse<EventAggregation>>, NotFound>> GetEventAggregationsByTypeAndPeriodAsync(IEventAggregationRepository<EventAggregation> repository, int eventType, int eventPeriod)
    {
        var eventAggregation = await repository.GetLatestAsync((EventType)eventType, (EventPeriod)eventPeriod);

        if (eventAggregation == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(new ApiResponse<EventAggregation>(eventAggregation, $"Latest Event Aggregation for eventType: {eventAggregation.EventType} and eventPeriod: {eventAggregation.EventPeriod}"));
    }

    private static async Task<Results<Ok<SentryEventsResponse>, NotFound, InternalServerError>> GetSentryEventAggregationsAsync(HttpContext context, ISentryHttpClient sentryHttpClient, SentrySettings sentrySettings, string dataset, string statsPeriod)
    {
        var response = await sentryHttpClient.GetEventsAsync(sentrySettings.OrganizationName, sentrySettings.ProjectId, sentrySettings.Environment, dataset, true, "count(span.duration)", statsPeriod, "span.name:\"root /\"");
        
        if (response == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(response);
    }
    
    private static async Task<StatusCodeHttpResult> BadSentryEventAggregationsAsync(HttpContext context, ISentryHttpClient sentryHttpClient)
    {
        await Task.CompletedTask;

        return TypedResults.StatusCode(429);
    }
}