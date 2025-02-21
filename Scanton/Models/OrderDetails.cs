using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scanton.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderNo { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]  
        public CustomerDetails CustomerDetails { get; set; } 
        public int ShippingAddressId { get; set; }
        public string Comment { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? ShippingCharges { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentMode { get; set; }
        
        public TimeSpan? OrderTime { get; set; }
        public string UpdateBy { get; set; }
       
    }

}
