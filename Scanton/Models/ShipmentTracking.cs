namespace Scanton.Models
{
    public class ShipmentTracking
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public string Status { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string Destination { get; set; }
    }
}
