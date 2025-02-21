using System.ComponentModel.DataAnnotations.Schema;

namespace Scanton.Models
{
    public class SoftBanner
    {

        public int Id { get; set; }
        public string? Banner_Image_1 { get; set; }
        public string? Banner_Image_2 { get; set; }
        public string? Banner_Image_3 { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }


        public bool Is_Active { get; set; }

        [NotMapped] 
        public IFormFile? Banner_1 { get; set; }

        [NotMapped]
        public IFormFile? Banner_2 { get; set; }

        [NotMapped]
        public IFormFile? Banner_3 { get; set; }

    }
}
