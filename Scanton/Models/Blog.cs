using System.ComponentModel.DataAnnotations;

namespace Scanton.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string? Blog_Category { get; set; }

        [StringLength(100)]
        public string Author { get; set; }

        public DateTime PublishedDate { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string? ImagePath { get; set; }
    }
}
