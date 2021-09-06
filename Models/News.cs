using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAPI.Models
{
    [Table("AllNews")]
    public class News
    {
        public Guid NewsId { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "Il Titolo non deve superare la lunghezza di 250")]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }
}