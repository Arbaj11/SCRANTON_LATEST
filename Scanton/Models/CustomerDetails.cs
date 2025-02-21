using System.ComponentModel.DataAnnotations;

namespace Scanton.Models
{
    public class CustomerDetails
    {
        [Key]
        public int CustomerID { get; set; }  // Corrected property definition

        public string Name { get; set; }
        public Guid? User_id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string ResellersLicense { get; set; }
        public string DistributorType { get; set; }
        public string Note { get; set; }
        public string? Attachment { get; set; }
        public DateTime? Created_date { get; set; }
        public bool? Is_Deleted { get; set; }
        public bool? Is_Active { get; set; }
    }

}
