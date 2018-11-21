using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookLibrary.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Genre")]
        [Required]
        [StringLength(100)]
        public string GenreName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}