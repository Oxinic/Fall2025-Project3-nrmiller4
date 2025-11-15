using System.ComponentModel.DataAnnotations;

namespace Fall2025_Project3_nrmiller4.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [Range(1, 150)]
        public int Age { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "IMDB Link")]
        public string ImdbLink { get; set; } = string.Empty;

        public byte[]? Photo { get; set; }

        // Navigation property
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}
