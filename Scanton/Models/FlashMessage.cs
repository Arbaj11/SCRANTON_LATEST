using System.ComponentModel.DataAnnotations;

namespace Scanton.Models
{
    public class FlashMessage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        [StringLength(255)]
        public string? ImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
    }


}
