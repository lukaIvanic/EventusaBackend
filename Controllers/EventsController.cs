using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventusaBackend.Models;
using EventusaBackend.Models.Users;
using Microsoft.CodeAnalysis;
using EventusaBackend.CalendarUtils;
using System.Collections;
using Microsoft.Extensions.Logging;

namespace EventusaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventContext _context;

        public EventsController(EventContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            if (_context.Events == null)
            {
                return NotFound();
            }



            return await _context.GetEventsExcludingFinished(DateTimeOffset.Now.ToUnixTimeSeconds());




        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @newEvent)
        {
            if (id != @newEvent.eventId)
            {
                return BadRequest();
            }

            if (!validateDateTime(@newEvent))
            {
                return BadRequest();
            }

            _context.Entry(@newEvent).State = EntityState.Modified;


            UpdateCalendarEvent(@newEvent);

            try
            {
                await _context.SaveChangesAsync();


            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private async Task UpdateCalendarEvent(Event @newEvent)
        {
            var oldEvent = _context.Events.Find(@newEvent.eventId);

            if (oldEvent != null && oldEvent.isInCalendar == 1)
            {

                if (@newEvent.isInCalendar == 1)
                {

                    RemoveFromCalendar(oldEvent);
                    PostEventToCalendar(@newEvent);

                }
                else
                {
                    RemoveFromCalendar(oldEvent);
                }
            }
            else if (@newEvent.isInCalendar == 1)
            {
                PostEventToCalendar(@newEvent);
            }

            if (oldEvent != null)
            {
                await _context.SaveChangesAsync();
            }


        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'EventContext.Events'  is null.");
            }

            if (!validateDateTime(@event))
            {
                return BadRequest();
            }

            _context.Events.Add(@event);

            if (@event.isInCalendar == 1)
            {
                PostEventToCalendar(@event);
            }


            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = @event.eventId }, @event);
        }

        private void PostEventToCalendar(Event @event)
        {

            DateTime startDateTime = DateTimeOffset.FromUnixTimeSeconds(@event.startDateTime).LocalDateTime.AddHours(1);
            DateTime endDateTime = DateTimeOffset.FromUnixTimeSeconds(@event.endDateTime).LocalDateTime.AddHours(1);

            Calendar.Generiraj(@event.eventId, startDateTime, endDateTime, @event.location ?? "", @event.title, @event.summary ?? "");
        }

        // DELETE: api/Events/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);

            if (@event.isInCalendar == 1)
            {
                RemoveFromCalendar(@event);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void RemoveFromCalendar(Event @event)
        {
            DateTime startDateTime = DateTimeOffset.FromUnixTimeSeconds(@event.startDateTime).LocalDateTime.AddHours(1);
            DateTime endDateTime = DateTimeOffset.FromUnixTimeSeconds(@event.endDateTime).LocalDateTime.AddHours(1);
            Calendar.Cancel(@event.eventId, startDateTime, endDateTime, @event.location ?? "", @event.title, @event.summary ?? "");
        }

        private bool EventExists(int eventId)
        {
            return (_context.Events?.Any(e => e.eventId == eventId)).GetValueOrDefault();
        }

        private bool validateDateTime(Event @event)
        {
            return @event.endDateTime >= DateTimeOffset.Now.ToUnixTimeSeconds();
        }
    }
}
