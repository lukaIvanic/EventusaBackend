using EventusaBackend.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EventusaBackend.Models.Events
{
    [Table("EVENTI")]
    public class Event
    {

        [Key]
        public int IDEventa { get; set; }
        public string Naslov { get; set; }
        public DateTime DatumVrijemeOd { get; set; }
        public DateTime DatumVrijemeDo { get; set; }
        public string Lokacija { get; set; }
        public string Opis { get; set; }
        public bool Kalendar { get; set; } // 1 -> true, 0 -> false
        public byte? eventColor { get; set; }
        public string? userIdsList { get; set; }


    }
}
