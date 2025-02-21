namespace Scanton.Models
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public int OrderNo { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StoreName { get; set; }
        public string StoreZipCode { get; set; }
        public decimal TotalPriceWithQuantity { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
    }

}
