namespace Integrations.Core.Models;

public class EventAggregation : BaseAuditableEntity
{
    public EventType EventType { get; set; }
    public EventPeriod EventPeriod { get; set; }
    public long Count { get; set; }
}