using Integrations.Core.Contracts;
using Integrations.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Integrations.Infrastructure.Repositories;

public class EventAggregationRepository(ApplicationDbContext context) : IEventAggregationRepository<EventAggregation>
{
    public async Task<IEnumerable<EventAggregation>> GetAllAsync(EventType eventType, EventPeriod eventPeriod)
    {
        var contextEventAggregations = context.EventAggregations.AsQueryable();
        
        if (eventType != EventType.All)
        {
            contextEventAggregations = contextEventAggregations.Where(x => x.EventType == eventType);
        }
        
        contextEventAggregations = contextEventAggregations.Where(x => x.EventPeriod == eventPeriod);
        
        return await contextEventAggregations.AsNoTracking().ToListAsync();
    }

    public async Task<EventAggregation?> GetLatestAsync(EventType eventType, EventPeriod eventPeriod)
    {
        return await context.EventAggregations
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.EventType == eventType && x.EventPeriod == eventPeriod);
    }

    public async Task<EventAggregation> AddEventAsync(EventAggregation eventAggregation)
    {
        var result = await context.EventAggregations.AddAsync(eventAggregation);

        await context.SaveChangesAsync();

        return result.Entity;
    }
}