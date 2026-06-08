using FluentValidation;
using Integrations.Sentry.Functions.Models;

namespace Integrations.Sentry.Functions.Validators;

public class EventAggregationRequestValidator : AbstractValidator<EventAggregationRequest>
{
    public EventAggregationRequestValidator()
    {
        RuleFor(x => x).NotNull();
        RuleFor(x => x.Dataset).NotEmpty();
        RuleFor(x => x.StatsPeriod).NotEmpty();
    }
}