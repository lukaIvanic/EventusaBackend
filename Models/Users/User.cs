using System.ComponentModel.DataAnnotations.Schema;

namespace EventusaBackend.Models.Users
{
    [Table("users_table")]
    public class User
    {
        public int userId {  get; set; }
        public string displayName { get; set; }
        public string username { get; set; }
        public string pass { get; set; }


    }
}
