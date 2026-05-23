namespace Integrations.Core.Models;

public enum EventPeriod
{
    LastHour,
    Last6Hours,
    Last12Hours,
    Last24Hours,
    Last2Days,
    Last5Days,
    Last7Days,
    Last14Days,
    Last30Days,
    // Last90Days // Need paid account with Sentry for this or higher
}