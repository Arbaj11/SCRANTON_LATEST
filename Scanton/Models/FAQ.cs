using System.ComponentModel.DataAnnotations;

namespace Scanton.Models
{
    public class FAQ
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Question is required.")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Answer is required.")]
        public string Answer { get; set; }



        [Required]
        public string FAQs_Type { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
