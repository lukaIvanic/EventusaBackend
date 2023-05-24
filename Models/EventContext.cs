using Microsoft.EntityFrameworkCore;

namespace EventusaBackend.Models
{
    public class EventContext : DbContext
    {

        public EventContext(DbContextOptions<EventContext> options) : base(options)
        {

        }

        public DbSet<Event> Events { get; set; } = null;


        public async Task<List<Event>> GetEventsExcludingFinished(long nowSecondsSinceEpoch)
        {
            return await Events.Where(e => e.endDateTime > nowSecondsSinceEpoch).ToListAsync();
        }

    }
}
