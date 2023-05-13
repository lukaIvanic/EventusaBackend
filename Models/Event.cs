using System.ComponentModel.DataAnnotations.Schema;


namespace EventusaBackend.Models
{
    [Table("events_table")]
    public class Event
    {

        public int eventId { get; set; }
        public string title { get; set; }
        public long startDateTime { get; set; }
        public long endDateTime { get; set;}
        public string? location { get; set; }
        public string? summary { get; set; }
        public int? isInCalendar { get; set; } // 1 -> true, 0 -> false
        public int? eventColor { get; set; }

    }
}
