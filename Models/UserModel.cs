using System.ComponentModel.DataAnnotations;

namespace NewsAPI.Models
{
    public class UserModel
    {
        [Required]
        [StringLength(8, ErrorMessage = "Il Nome non deve superare la lunghezza di 8")]
        public string Username { get;  set; }

        [Required]
        public string Password { get;  set; }
    }
}