using System.ComponentModel.DataAnnotations;

namespace Scanton.Models
{
    public class Banner
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string BannerUrl { get; set; }

        public string? ImagePath { get; set; }
        public string? TittleParagraph1 { get; set; }
        public string? TittleParagraph2 { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
