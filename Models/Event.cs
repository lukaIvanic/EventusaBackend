using System.ComponentModel.DataAnnotations.Schema;


namespace EventusaBackend.Models
{
    [Table("events_table")]
    public class Event
    {

        public int Id { get; set; }
        public string Name { get; set; }

    }
}
