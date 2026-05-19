using Integrations.Core.Contracts;
using Integrations.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Integrations.Infrastructure.Repositories;

public class EventAggregationRepository(ApplicationDbContext context) : IEventAggregationRepository<EventAggregation>
{
    public async Task<IEnumerable<EventAggregation>> GetAllAsync()
    {
        return await context.EventAggregations.AsNoTracking().ToListAsync();
    }

    public async Task<EventAggregation> AddEventAsync(EventAggregation eventAggregation)
    {
        var result = await context.EventAggregations.AddAsync(eventAggregation);
        
        await context.SaveChangesAsync();
        
        return result.Entity;
    }
}