using Integrations.Core.Models;

namespace Integrations.Core.Contracts;

public interface IEventAggregationRepository<T> where T : EventAggregation
{
    Task<IEnumerable<T>> GetAllAsync(EventType eventType, EventPeriod eventPeriod);
    Task<T?> GetLatestAsync(EventType eventType, EventPeriod eventPeriod);
    Task<T> AddEventAsync(T eventAggregation);
}