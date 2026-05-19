using Integrations.Core.Models;

namespace Integrations.Api.Features.EventAggregations;

public class EventAggregation
{
    public int Id { get; set; }
    public EventType EventType { get; set; }
    public EventPeriod EventPeriod { get; set; }
    public long Count { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastModifiedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
}