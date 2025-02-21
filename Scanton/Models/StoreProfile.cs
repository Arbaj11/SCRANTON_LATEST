namespace Scanton.Models
{
    public class StoreProfile
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }

        public string? ImagePath { get; set; }
    }

}
