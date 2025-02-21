using System.ComponentModel.DataAnnotations;

namespace Scanton.Models
{
    public class COA
    {
        public int Id { get; set; }

        
        public string? Product_name { get; set; }

        
        public string? Product_image { get; set; } // Path for the uploaded image

        
        [RegularExpression(@".*\.pdf$", ErrorMessage = "Only PDF files are allowed.")]
        public string? Pdf_Path { get; set; } // Path for the uploaded PDF
        public DateTime? Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
        public bool Is_Active { get; set; }
    }
}
