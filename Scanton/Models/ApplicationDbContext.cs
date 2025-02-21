using Microsoft.EntityFrameworkCore;

namespace Scanton.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ParentCategory> ParentCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<COA> COA { get; set; }
        
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<StoreProfile> StoreProfiles { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<ShipmentTracking> ShipmentTrackings { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<FlashMessage> FlashMessages { get; set; }
        public DbSet<CustomerDetails> CustomerDetails { get; set; }

        public DbSet<SoftBanner> SoftBanner { get; set; }

        public DbSet<ShortBannner> ShortBanner { get; set; }
        public DbSet<FooterDescription> FooterDescription { get; set; }
        public DbSet<SocialMedia> SocialMedia { get; set; }
        public DbSet<Page> Page { get; set; }
        public DbSet<UserRating> UserRating { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<VideoGallery> VideoGallery { get; set; }
        public DbSet<OrderDetailsMaster> OrderDetailsMaster { get; set; }


    }
}
