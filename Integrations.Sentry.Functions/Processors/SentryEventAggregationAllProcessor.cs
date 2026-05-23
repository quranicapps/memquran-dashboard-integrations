using Integrations.Core;
using Integrations.Core.Contracts;
using Integrations.Core.Models;
using Integrations.Sentry.Functions.Contracts;
using Integrations.Sentry.Functions.Features.EventAggregations;
using Integrations.Sentry.Functions.Models;
using Integrations.Sentry.Functions.Settings;
using Microsoft.Extensions.Logging;

namespace Integrations.Sentry.Functions.Processors;

/// <summary>
/// This processor will get all the events from Sentry and store them in the database, free account is upto 30 days
/// </summary>
public class SentryEventAggregationAllProcessor(
    ISentryService sentryService,
    SentrySettings sentrySettings,
    IEventAggregationRepository<EventAggregation> repository,
    ILogger<SentryEventAggregationAllProcessor> logger) : IProcessor<Result<EventAggregationProcessorResponse>>
{
    public async Task<Result<EventAggregationProcessorResponse>> ProcessAsync()
    {
        var totalInsertedRecords = 0;
        foreach (var period in sentrySettings.EventAggregationPeriods)
        {
            var result = await sentryService.GetSentryEventAggregationSpanCountAsync(period, "span.name:\"root /\"");

            if (!result.IsSuccess)
            {
                return Result<EventAggregationProcessorResponse>.Failure(result.ErrorMessage);
            }

            var count = result.Value;

            logger.LogInformation("***** Count is: {Count} *****", count);

            var eventPeriod = EventAggregationPeriodMapper.Map(period);
            await repository.AddEventAsync(new EventAggregation
            {
                Count = count,
                EventType = EventType.Spans,
                EventPeriod = eventPeriod,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = $"({eventPeriod}) {nameof(SentryEventAggregationAllProcessor)}",
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedBy = $"({eventPeriod}) {nameof(SentryEventAggregationAllProcessor)}"
            });
            
            totalInsertedRecords++;
        }

        return Result<EventAggregationProcessorResponse>.SuccessObjectResult(new EventAggregationProcessorResponse { RecordsProcessed = totalInsertedRecords });
    }
}