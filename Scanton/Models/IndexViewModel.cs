using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scanton.Models
{
    public class IndexViewModel
    {
        public List<Banner> Banners { get; set; } = new List<Banner>();
        public List<SoftBanner>SoftBanners { get; set; } = new List<SoftBanner>();
        public List<ShortBanner>ShortBanners { get; set; } = new List<ShortBanner>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<ParentCategory> ParentCategories { get; set; } = new List<ParentCategory>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Blog> Blogs { get; set; } = new List<Blog>();
        public List<StoreProfile> StoreProfiles { get; set; } = new List<StoreProfile>();
        public List<AboutUs> Aboutus { get; set; } = new List<AboutUs>();
        public List<UserRating> UserRatings { get; set; } = new List<UserRating>();
        public List<VideoGallery> VideoGalleries { get; set; } = new List<VideoGallery>();
        public List<FAQ> FAQs { get; set; } = new List<FAQ>();
        public List<COA> COAs { get; set; } = new List<COA>();
        public List<Product> JustArrivedProducts { get; set; }
        public List<OrderDetailsMaster> OrderDetailsMasters { get; set; }

        public class Banner
        {
            public string ImagePath { get; set; }
            public string Title { get; set; }
            public string? TittleParagraph1 { get; set; }
            public string? TittleParagraph2 { get; set; }

        }

        public class OrderDetailsMaster
        {
           
            public int OrderDetailsMasterId { get; set; }

           
            public int OrderId { get; set; }


            public int ProductId { get; set; }
            public int TotalProducts { get; set; }

           
            public int StoreId { get; set; }


            public string OrderStatus { get; set; }
            public int OrderNo { get; set; }


            public string StoreName { get; set; }
           

            public string StoreZipCode { get; set; }


            public int Quantity { get; set; }

         
            [Column(TypeName = "decimal(10,2)")]
            public decimal ProductPrice { get; set; }

        
            [Column(TypeName = "decimal(10,2)")]
            public decimal TotalPriceWithQuantity { get; set; }

  
            public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

            public Guid? User_id { get; set; }

            // Navigation properties
            [ForeignKey("OrderId")]
            public virtual OrderDetails OrderDetails { get; set; }

            [ForeignKey("ProductId")]
            public virtual Product Product { get; set; }
        }

        public class COA
        {
            public int Id { get; set; }


            public string Product_name { get; set; }


            public string Product_image { get; set; } 

           


            
            public string Pdf_Path { get; set; } // Path for the uploaded PDF
            public DateTime Created_Date { get; set; }
            public DateTime Updated_Date { get; set; }
            public bool Is_Active { get; set; }
        }

        public class Category
        {
            public int Id { get; set; }

            public string Name { get; set; }


            public string? ImagePath { get; set; }
            public string? ParentCategoryName { get; set; }

            public int ParentCategoryId { get; set; }
            public ParentCategory ParentCategory { get; set; }



        }

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
            public string ImagePath { get; set; }
        }

        public class Product
        {

            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string? TittleBadge { get; set; }
            public string? PriceWithDiscount { get; set; }
            public string? ImagePath { get; set; }
            public string? SubImage_1 { get; set; }
            public string? SubImage_2 { get; set; }
            public string? SubImage_3 { get; set; }
            public string? SubImage_4 { get; set; }
            public string Flavor { get; set; }
            public string FeaturedProducts { get; set; }
            public string JustArrivedProducts { get; set; }
            public bool BadgeIsActive { get; set; }
            public string Description { get; set; }
            public decimal Weight { get; set; }
            public string? ShortDescription { get; set; }
            public decimal PricePerPcs { get; set; }
            public DateTime Created_Date { get; set; }

            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
        }

        public class CustomerDetail
        {
            
            public int CustomerID { get; set; } 

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
            
        }

        public class AboutUs
        {
            public int Id { get; set; }
            public string? Banner_Image { get; set; }
            public string Banner_Title { get; set; }
            public string Banner_SubTitle { get; set; }
            public string? Product_Image1 { get; set; }
            public string Product1_Title { get; set; }
            public string Product1_Description { get; set; }
            public string? Product_Image2 { get; set; }
            public string Product2_Title { get; set; }
            public string Product2_Description { get; set; }
            public string? VideoPath { get; set; }
            public string VideoTitle { get; set; }
            public DateTime Created_Date { get; set; }
            public DateTime? Updated_Date { get; set; }
            public bool Is_Active { get; set; }
        }

        public class SoftBanner
        {
            public string? Banner_Image_1 { get; set; }
            public string? Banner_Image_2 { get; set; }
            public string? Banner_Image_3 { get; set; }
            public bool Is_Active { get; set; }

        }


        public class ShortBanner
        {
            public string? Short_Banner { get; set; }
            public string? Heading_Tittle { get; set; }
            public string? Sub_Tittle_Heading { get; set; }
            public bool Is_Active { get; set; }

        }

        public class Blog
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string Author { get; set; }
            public string? ImagePath { get; set; }
            public DateTime PublishedDate { get; set; } = DateTime.Now;
            public string Blog_Category { get; set; }
          

        }

        public class SocialMedia
        {
           
            public string AccountURLLink { get; set; }
            public string AccountName { get; set; }
            public string AccountIcon { get; set; }
            public bool Is_Active { get; set; }


        }
        public class Page
        {
            public int CustomPageld { get; set; }
            public string CustomePageTittle { get; set; }
            public string CustomePageData { get; set; }
            public string? PageUrl { get; set; }
            public string? PageCategory { get; set; }
            public bool Is_Active { get; set; }

        }

        public class FAQ
        {
            public int Id { get; set; }

           
            public string Question { get; set; }

           
            public string Answer { get; set; }


            public string FAQs_Type { get; set; }

            public bool IsActive { get; set; }

            public DateTime? CreatedAt { get; set; }

        }

        public class FooterDescription
        {

            
            public string Title { get; set; }

            
            public string Description { get; set; }



            public bool Is_Active { get; set; } = true;

          
        }

        public class FlashMessage
        {


            public string Title { get; set; }
            public string Message { get; set; }
          
            public string? ImagePath { get; set; }
            
            public bool IsActive { get; set; }


        }

        public class ParentCategory
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string? ImagePath { get; set; }
            public string Description { get; set; }
            public ICollection<Category> Categories { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.Now;

           
        }
        public class UserRating
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string Comments { get; set; }
            public int Rating { get; set; }
            public bool Is_Active { get; set; }


        }

        public class VideoGallery
        {
            public int Id { get; set; }         // Primary Key
            public string Title { get; set; }   // Title of the video
            public string SubTitle { get; set; } // Sub-title of the video
            public string? VideoPath { get; set; } // Path to the video file
            public bool Is_Active { get; set; }  // Status of the video

        }

    }
}
