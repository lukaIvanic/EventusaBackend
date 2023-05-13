using Microsoft.EntityFrameworkCore;

namespace EventusaBackend.Models
{
    public class EventContext : DbContext
    {

        public EventContext(DbContextOptions<EventContext> options) : base(options)
        {

        }

        public DbSet<Event> Events { get; set; } = null;

    }
}
