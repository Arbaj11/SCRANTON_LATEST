namespace Scanton.Models
{
    public class OrderMaster
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderNo { get; set; }
        public int CustomerId { get; set; }
        public int ShippingAddressId { get; set; }
        public string Comment { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingCharges { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentId { get; set; }
        public string TrackingId { get; set; }
        public DateTime OrderTime { get; set; }
        public string UpdateBy { get; set; }
        public int CouponId { get; set; }
    }

}
