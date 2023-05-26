using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventusaBackend.Models.Users
{
    [Table("USERS")]
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? Mail { get; set; }
        public string? Pass { get; set; }


    }
}
