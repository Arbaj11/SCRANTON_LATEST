using System.ComponentModel.DataAnnotations;

namespace Scanton.Models
{
    public class ParentCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
        public string? ImagePath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Category> Categories { get; set; }
    }
}
