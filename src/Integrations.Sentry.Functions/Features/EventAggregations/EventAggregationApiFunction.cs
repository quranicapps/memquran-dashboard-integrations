using Integrations.Core;
using Integrations.Core.Contracts;
using Integrations.Core.Models;
using Integrations.Sentry.Functions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Integrations.Sentry.Functions.Features.EventAggregations;

public class EventAggregationApiFunction(
    IProcessorFactory<IProcessor<Result<EventAggregationProcessorResponse>>> processorFactory,
    ILogger<EventAggregationApiFunction> logger)
{
    [Function(nameof(EventAggregationApiFunction))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = $"{nameof(EventAggregationApiFunction)}/process")] HttpRequest request)
    {
        logger.LogInformation("C# HTTP trigger function processed a request");

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

        return new CreatedResult(string.Empty, new ApiResponse<string>(null, "Completed processing event aggregations."));
    }
}