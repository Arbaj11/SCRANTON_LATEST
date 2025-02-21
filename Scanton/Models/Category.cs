using System.ComponentModel.DataAnnotations;

namespace Scanton.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? ImagePath { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int ParentCategoryId { get; set; }
        public ParentCategory? ParentCategory { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Product>? Products { get; set; }
    }
}
