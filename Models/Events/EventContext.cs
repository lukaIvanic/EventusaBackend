using Microsoft.EntityFrameworkCore;

namespace EventusaBackend.Models.Events
{
    public class EventContext : DbContext
    {

        public EventContext(DbContextOptions<EventContext> options) : base(options)
        {

        }

        public DbSet<Event> Events { get; set; } = null;


        public async Task<List<Event>> GetEventsExcludingFinished(DateTimeOffset dateTimeNow)
        {
            return await Events.Where(e => e.DatumVrijemeDo > dateTimeNow).ToListAsync();

        }

    }
}
