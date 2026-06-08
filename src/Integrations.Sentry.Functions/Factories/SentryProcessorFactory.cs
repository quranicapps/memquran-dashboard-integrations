using Integrations.Core;
using Integrations.Core.Contracts;
using Integrations.Sentry.Functions.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Integrations.Sentry.Functions.Factories;

public class SentryProcessorFactory(IServiceProvider serviceProvider) : IProcessorFactory<IProcessor<Result<EventAggregationProcessorResponse>>>
{
    public IEnumerable<IProcessor<Result<EventAggregationProcessorResponse>>> CreateProcessors()
    {
        var processors = serviceProvider.GetServices<IProcessor<Result<EventAggregationProcessorResponse>>>().ToList();
        
        return processors.Count == 0 
            ? throw new InvalidOperationException("No processors found.") 
            : processors;
    }
}