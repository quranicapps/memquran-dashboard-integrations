using Integrations.Core.Models;

namespace Integrations.Core.Contracts;

public interface IEventAggregationRepository<T> where T : EventAggregation
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddEventAsync(T eventAggregation);
}