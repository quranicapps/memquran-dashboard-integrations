using Integrations.Core;
using Integrations.Core.Contracts;
using Integrations.Sentry.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Integrations.Sentry.Functions.Features.EventAggregations;

public class EventAggregationTimerFunction(
    IProcessorFactory<IProcessor<Result<EventAggregationProcessorResponse>>> processorFactory,
    ILogger<EventAggregationTimerFunction> logger)
{
    [Function(nameof(EventAggregationTimerFunction))]
    public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer)
    {
        logger.LogInformation("***** C# Timer trigger function executed at: {DateTime}", DateTime.Now);

        if (myTimer.ScheduleStatus is not null)
        {
            logger.LogInformation("Next timer schedule at: {ScheduleStatusNext}", myTimer.ScheduleStatus.Next.ToLocalTime());
        }

        var processors = processorFactory.CreateProcessors();
        
        foreach (var processor in processors)
        {
            var result = await processor.ProcessAsync();

            if (!result.IsSuccess)
            {
                logger.LogError("Failed to process response for processor: {ProcessorName}. {ErrorMessage}", processor.GetType().Name, result.ErrorMessage);
            }
            else
            {
                logger.LogInformation("Successfully processed response for processor: {ProcessorName}. Records processed: {RecordsProcessed}", processor.GetType().Name, result.Value.RecordsProcessed);
            }
        }
        
        logger.LogInformation("*****");
    }
}