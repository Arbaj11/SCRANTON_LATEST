using System.ComponentModel.DataAnnotations;

namespace Scanton.Models
{
    public class FooterDescription
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(1500)]
        public string Description { get; set; }

       

        public bool Is_Active { get; set; } = true;

        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
    }
}
