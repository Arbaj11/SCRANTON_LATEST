namespace Scanton.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddMonths(1); // Set a valid default expiry date.
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Set a valid default creation date.
    }
}
