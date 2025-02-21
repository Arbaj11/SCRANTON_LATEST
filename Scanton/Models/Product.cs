using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scanton.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 100000, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "MSP is required.")]
        public decimal MSP { get; set; }

        [Required(ErrorMessage = "Weight is required.")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Price Per Pcs is required.")]
        public decimal PricePerPcs { get; set; }


        public string Stain { get; set; }
        public string Flavor { get; set; }
        public bool BadgeIsActive { get; set; }


        [Required(ErrorMessage = "Just Arrived  Product is required.")]
        public string JustArrivedProducts { get; set; }

        [Required(ErrorMessage = "Featured products  is required.")]
        public string FeaturedProducts { get; set; }


        [Required(ErrorMessage = "Pack Size is required.")]
        public string PackSize { get; set; }

        public string? StoreZipCode { get; set; }


        public string? StoreName { get; set; }

        [Required(ErrorMessage = "Badge is required.")]
        public string? TittleBadge { get; set; }

        [Required(ErrorMessage = "Desccription is required.")]
        public string? ShortDescription { get; set; }

        [Required(ErrorMessage = "Parent Category is required.")]
        public int ParentCategoryId { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }


        [Required(ErrorMessage = "StoreProfile is required.")]
        public int StoreProfileId { get; set; }

        public string? PriceWithDiscount { get; set; }

        public string? ImagePath { get; set; }
        public string? SubImage_1 { get; set; }
        public string? SubImage_2 { get; set; }
        public string? SubImage_3 { get; set; }
        public string? SubImage_4 { get; set; }
        


    }

}
