using Integrations.Core.Models;

namespace Integrations.Sentry.Functions.Features.EventAggregations;

public static class EventAggregationPeriodMapper
{
    public static EventPeriod Map(string period)
    {
        return period.ToLower() switch
        {
            "1h" => EventPeriod.LastHour,
            "6h" => EventPeriod.Last6Hours,
            "12h" => EventPeriod.Last12Hours,
            "24h" => EventPeriod.Last24Hours,
            "2d" => EventPeriod.Last2Days,
            "5d" => EventPeriod.Last5Days,
            "7d" => EventPeriod.Last7Days,
            "14d" => EventPeriod.Last14Days,
            "30d" => EventPeriod.Last30Days,
            // "90d" => EventPeriod.Last90Days,
            _ => throw new ArgumentException($"Invalid period: {period}")
        };
    }
}