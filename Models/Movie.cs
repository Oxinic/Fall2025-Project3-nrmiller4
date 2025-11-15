using System.ComponentModel.DataAnnotations;

namespace Fall2025_Project3_nrmiller4.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Display(Name = "IMDB Link")]
        public string ImdbLink { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Genre { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Year")]
        public int YearOfRelease { get; set; }

        public byte[]? Poster { get; set; }

        // Navigation property
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}
