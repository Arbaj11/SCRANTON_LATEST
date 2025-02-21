using Scanton.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderDetailsMaster
{
    [Key]
    public int OrderDetailsMasterId { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    public int OrderNo { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int StoreId { get; set; }

    [Required]
    public string StoreName { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal ProductPrice { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPriceWithQuantity { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Guid? User_id { get; set; }

    // Navigation properties
    [ForeignKey("OrderId")]
    public virtual OrderDetails OrderDetails { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
}
