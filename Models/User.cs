using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore.MariaDB.Models
{
    public class Users
    {
        [Key]
        public int userid { get; set; }
        public string username { get; set; }
        public string email { get; set; }
    }
}
