using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookLibrary.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [Display(Name = "Release date")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "Writer")]
        [Required]
        public int WriterId { get; set; }
        public virtual Writer Writer { get; set; }

        [Display(Name = "Genre")]
        [Required]
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

    }
}