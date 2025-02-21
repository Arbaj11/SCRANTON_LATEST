using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Scanton.Models;
using System.Diagnostics;
using System.Security.Claims;
using static Scanton.Models.IndexViewModel;
using System.Linq;

namespace Scanton.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        //http://techqunba-001-site5.atempurl.com/
        public async Task<IActionResult> Index()
        {
            // Get all products and banners from the database
            //var products = await _context.Products.ToListAsync();


            List<int> cart = HttpContext.Session.GetObjectFromJson<List<int>>("Cart") ?? new List<int>();
            var cartProducts = await _context.Products.Where(p => cart.Contains(p.Id)).ToListAsync();
            var banners = await _context.Banners.ToListAsync();
            var flash = await _context.FlashMessages.ToListAsync();
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var softbanners = await _context.SoftBanner.ToListAsync();
            var shortbanners = await _context.ShortBanner.ToListAsync();
            var parantCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var recentProducts = await _context.Products.OrderByDescending(rp => rp.Created_Date).FirstOrDefaultAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var rating = await _context.UserRating.ToListAsync();
            var video = await _context.VideoGallery.ToListAsync();
            var recentBlog = await _context.Blogs.OrderByDescending(rb => rb.Id).FirstOrDefaultAsync();

            var cartproductCount = cartProducts.Count();

            var bannerViewModels = banners.Select(b => new IndexViewModel.Banner
            {
                ImagePath = b.ImagePath,
                Title = b.Title,
                TittleParagraph1 = b.TittleParagraph1,
                TittleParagraph2 = b.TittleParagraph2


            }).ToList();


            var flashMessageViewModel = flash.Select(b => new IndexViewModel.FlashMessage
            {
                ImagePath = b.ImagePath,
                IsActive = b.IsActive

            }).ToList();

            var footerIconViewModels = footersIcon.Select(s => new IndexViewModel.SocialMedia
            {
                AccountIcon = s.AccountIcon,
                AccountName = s.AccountURLLink,
                AccountURLLink = s.AccountURLLink,
                Is_Active = s.Is_Active


            }).ToList();

            var footerDescriptionViewModels = footersDescription.Select(f => new IndexViewModel.FooterDescription
            {
                Title = f.Title,
                Description = f.Description,
                Is_Active = f.Is_Active


            }).ToList();

            var footerPagesViewModels = footerPages.Select(p => new IndexViewModel.Page
            {
                CustomPageld = p.CustomPageld,
                CustomePageData = p.CustomePageData,
                CustomePageTittle = p.CustomePageTittle,
                PageCategory = p.PageCategory,
                PageUrl = p.PageUrl,
                Is_Active = p.Is_Active
            }).ToList();

            var categoriess = _context.Categories
           .Include(c => c.ParentCategory) // Eagerly load the ParentCategory
           .ToList();

            var categoryViewModels = categories.Select(c => new IndexViewModel.Category
            {
                Id = c.Id,
                ImagePath = c.ImagePath,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategoryName = c.ParentCategory != null ? c.ParentCategory.Name : "No Parent"
            }).ToList();

            var ParentCategoryViewModels = parantCategories?.Select(pc => new IndexViewModel.ParentCategory
            {
                Id = pc.Id,
                Name = pc.Name,
                Description = pc.Description,
                ImagePath = pc.ImagePath,
                Categories = pc.Categories?.Select(c => new IndexViewModel.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagePath = c.ImagePath,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = pc.Name
                }).ToList() ?? new List<IndexViewModel.Category>()
            }).ToList() ?? new List<IndexViewModel.ParentCategory>();



            var productViewModels = products.Select(p => new IndexViewModel.Product
            {
                Id = p.Id,
                Name = p.Name,
                ImagePath = p.ImagePath,
                TittleBadge = p.TittleBadge,
                PriceWithDiscount = p.PriceWithDiscount,
                Price = p.Price,
                Flavor = p.Flavor,
                BadgeIsActive = p.BadgeIsActive,
                FeaturedProducts = p.FeaturedProducts,
                SubImage_1 = p.SubImage_1
            }).ToList();

            var userRatingViewModels = rating.Select(p => new IndexViewModel.UserRating
            {
                Id = p.Id,
                UserName = p.UserName,
                Rating = p.Rating,
                Comments = p.Comments,
                Is_Active = p.Is_Active
            }).ToList();

            var RecentBlogs = new IndexViewModel.Blog
            {
                ImagePath = recentBlog.ImagePath,
                Author = recentBlog.ImagePath,
                Content = recentBlog.Content,
                Title = recentBlog.Title,
                Blog_Category = recentBlog.Blog_Category
            };


            var RecentProductViewModels = new IndexViewModel.Product
            {
                Id = recentProducts.Id,
                Name = recentProducts.Name,
                ImagePath = recentProducts.ImagePath,
                TittleBadge = recentProducts.TittleBadge,
                PriceWithDiscount = recentProducts.PriceWithDiscount,
                Price = recentProducts.Price,
                Flavor = recentProducts.Flavor,
                SubImage_1 = recentProducts.SubImage_1,
                SubImage_2 = recentProducts.SubImage_2,
                SubImage_3 = recentProducts.SubImage_3,
                SubImage_4 = recentProducts.SubImage_4,
                BadgeIsActive = recentProducts.BadgeIsActive,
                Weight = recentProducts.Weight,
                PricePerPcs = recentProducts.PricePerPcs,
                FeaturedProducts = recentProducts.FeaturedProducts,
                CategoryId = recentProducts.CategoryId,
                Description = recentProducts.Description,
                ShortDescription = recentProducts.ShortDescription,
                // Assuming categories is already fetched, and you want the category name
                CategoryName = categories.FirstOrDefault(c => c.Id == recentProducts.CategoryId)?.Name ?? "Unknown"
            };



            var softBannerViewModel = softbanners.Select(sb => new IndexViewModel.SoftBanner
            {
                Banner_Image_1 = sb.Banner_Image_1,
                Banner_Image_2 = sb.Banner_Image_2,
                Banner_Image_3 = sb.Banner_Image_3,
                Is_Active = sb.Is_Active
            }).ToList();

            var shortBannerViewModel = shortbanners.Select(shb => new IndexViewModel.ShortBanner
            {
                Heading_Tittle = shb.Heading_Tittle,
                Sub_Tittle_Heading = shb.Sub_Tittle_Heading,
                Short_Banner = shb.Short_Banner,
                Is_Active = shb.Is_Active
            }).ToList();

            var CartProducts = cartProducts.Select(p => new IndexViewModel.Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                TittleBadge = p.TittleBadge,
                PriceWithDiscount = p.PriceWithDiscount,
                ImagePath = p.ImagePath,
                SubImage_1 = p.SubImage_1,
                SubImage_2 = p.SubImage_2,
                SubImage_3 = p.SubImage_3,
                SubImage_4 = p.SubImage_4,
                Flavor = p.Flavor,
                FeaturedProducts = p.FeaturedProducts,
                JustArrivedProducts = p.JustArrivedProducts,
                BadgeIsActive = p.BadgeIsActive,
                Description = p.Description,
                Weight = p.Weight,
                ShortDescription = p.ShortDescription,
                PricePerPcs = p.PricePerPcs,
                Created_Date = p.Created_Date,
                CategoryId = p.CategoryId,

            }).ToList();


            var blogsViewModel = blogs.Select(b => new IndexViewModel.Blog
            {
                ImagePath = b.ImagePath,
                Author = b.ImagePath,
                Content = b.Content,
                Title = b.Title,
                Blog_Category = b.Blog_Category

            }).ToList();

            var videoViewModel = video.Select(b => new IndexViewModel.VideoGallery
            {
                Title = b.Title,
                SubTitle = b.SubTitle,
                VideoPath = b.VideoPath,
                Is_Active = b.Is_Active


            }).ToList();
            // Create a view model to pass to the view
            var viewModel = new IndexViewModel
            {
                Banners = bannerViewModels,
                Categories = categoryViewModels,
                Products = productViewModels,
                SoftBanners = softBannerViewModel,
                ShortBanners = shortBannerViewModel,
                Blogs = blogsViewModel,
                ParentCategories = ParentCategoryViewModels,
                UserRatings = userRatingViewModels,
                VideoGalleries = videoViewModel
            };
            ViewBag.RecentProduct = recentProducts;
            ViewBag.RecentBlogs = RecentBlogs;
            ViewBag.RecentProductId = recentProducts.Id;
            ViewBag.FlashImages = flashMessageViewModel;
            ViewBag.Banners = bannerViewModels;
            ViewBag.CardtProducts = CartProducts;
            ViewBag.FooterIcon = footerIconViewModels;
            TempData["TotalCartProducts"] = cartproductCount;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.FooterPage = footerPagesViewModels;
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Blogs()
        {
            var banners = await _context.Banners.ToListAsync();
            var flash = await _context.FlashMessages.ToListAsync();
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var softbanners = await _context.SoftBanner.ToListAsync();
            var shortbanners = await _context.ShortBanner.ToListAsync();
            var parantCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var recentProducts = await _context.Products.OrderByDescending(rp => rp.Created_Date).FirstOrDefaultAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var rating = await _context.UserRating.ToListAsync();
            var video = await _context.VideoGallery.ToListAsync();
            var recentBlog = await _context.Blogs.OrderByDescending(rb => rb.Id).FirstOrDefaultAsync();

            var ParentCategoryViewModels = parantCategories?.Select(pc => new IndexViewModel.ParentCategory
            {
                Id = pc.Id,
                Name = pc.Name,
                Description = pc.Description,
                ImagePath = pc.ImagePath,
                Categories = pc.Categories?.Select(c => new IndexViewModel.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagePath = c.ImagePath,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = pc.Name
                }).ToList() ?? new List<IndexViewModel.Category>()
            }).ToList() ?? new List<IndexViewModel.ParentCategory>();

            var footerIconViewModels = footersIcon.Select(s => new IndexViewModel.SocialMedia
            {
                AccountIcon = s.AccountIcon,
                AccountName = s.AccountURLLink,
                AccountURLLink = s.AccountURLLink,
                Is_Active = s.Is_Active


            }).ToList();
            var Blog = blogs.Select(f => new IndexViewModel.Blog
            {
                Id = f.Id,
                ImagePath = f.ImagePath,
                Author = f.Author,
                Content = f.Content,
                Title = f.Title,
                Blog_Category = f.Blog_Category,
                PublishedDate = f.PublishedDate
            }).ToList();

            var footerDescriptionViewModels = footersDescription.Select(f => new IndexViewModel.FooterDescription
            {
                Title = f.Title,
                Description = f.Description,
                Is_Active = f.Is_Active


            }).ToList();

            var footerPagesViewModels = footerPages.Select(p => new IndexViewModel.Page
            {
                CustomePageData = p.CustomePageData,
                CustomePageTittle = p.CustomePageTittle,
                PageCategory = p.PageCategory,
                PageUrl = p.PageUrl,
                Is_Active = p.Is_Active


            }).ToList();
            var viewModel = new IndexViewModel
            {

                ParentCategories = ParentCategoryViewModels,
                Blogs = Blog


            };

            ViewBag.FooterIcon = footerIconViewModels;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.FooterPage = footerPagesViewModels;

            return View(viewModel);
        }


        public async Task<IActionResult> BlogDetails(int blog_id)
        {
            var banners = await _context.Banners.ToListAsync();
            var flash = await _context.FlashMessages.ToListAsync();
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var softbanners = await _context.SoftBanner.ToListAsync();
            var shortbanners = await _context.ShortBanner.ToListAsync();
            var parantCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var recentProducts = await _context.Products.OrderByDescending(rp => rp.Created_Date).FirstOrDefaultAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var rating = await _context.UserRating.ToListAsync();
            var video = await _context.VideoGallery.ToListAsync();
            var recentBlog = await _context.Blogs.OrderByDescending(rb => rb.Id).ToListAsync();
            var blogDetails = await _context.Blogs.FirstOrDefaultAsync(bd => bd.Id == blog_id);

            var ParentCategoryViewModels = parantCategories?.Select(pc => new IndexViewModel.ParentCategory
            {
                Id = pc.Id,
                Name = pc.Name,
                Description = pc.Description,
                ImagePath = pc.ImagePath,
                Categories = pc.Categories?.Select(c => new IndexViewModel.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagePath = c.ImagePath,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = pc.Name
                }).ToList() ?? new List<IndexViewModel.Category>()
            }).ToList() ?? new List<IndexViewModel.ParentCategory>();

            var footerIconViewModels = footersIcon.Select(s => new IndexViewModel.SocialMedia
            {
                AccountIcon = s.AccountIcon,
                AccountName = s.AccountURLLink,
                AccountURLLink = s.AccountURLLink,
                Is_Active = s.Is_Active


            }).ToList();

            var footerDescriptionViewModels = footersDescription.Select(f => new IndexViewModel.FooterDescription
            {
                Title = f.Title,
                Description = f.Description,
                Is_Active = f.Is_Active


            }).ToList();

            var footerPagesViewModels = footerPages.Select(p => new IndexViewModel.Page
            {
                CustomePageData = p.CustomePageData,
                CustomePageTittle = p.CustomePageTittle,
                PageCategory = p.PageCategory,
                PageUrl = p.PageUrl,
                Is_Active = p.Is_Active


            }).ToList();

            var RecentBlogs = recentBlog.Select(rb => new IndexViewModel.Blog
            {
                Id = rb.Id,
                ImagePath = rb.ImagePath,
                Author = rb.Author,
                Content = rb.Content,
                Title = rb.Title,
                Blog_Category = rb.Blog_Category,
                PublishedDate = rb.PublishedDate

            }).ToList();

            var blogDetail = new IndexViewModel.Blog
            {
                Id = blogDetails.Id,
                ImagePath = blogDetails.ImagePath,
                Author = blogDetails.Author,
                Content = blogDetails.Content,
                Title = blogDetails.Title,
                Blog_Category = blogDetails.Blog_Category,
                PublishedDate = blogDetails.PublishedDate
            };
            var viewModel = new IndexViewModel
            {

                ParentCategories = ParentCategoryViewModels,
                Blogs = RecentBlogs
            };



            ViewBag.FooterIcon = footerIconViewModels;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.FooterPage = footerPagesViewModels;
            ViewBag.BlogDetals = blogDetail;

            return View(viewModel);
        }


        public async Task<IActionResult> AboutUs()
        {

            var banners = await _context.Banners.ToListAsync();
            var flash = await _context.FlashMessages.ToListAsync();
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var softbanners = await _context.SoftBanner.ToListAsync();
            var shortbanners = await _context.ShortBanner.ToListAsync();
            var parantCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var recentProducts = await _context.Products.OrderByDescending(rp => rp.Created_Date).FirstOrDefaultAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var rating = await _context.UserRating.ToListAsync();
            var video = await _context.VideoGallery.ToListAsync();
            var recentBlog = await _context.Blogs.OrderByDescending(rb => rb.Id).FirstOrDefaultAsync();
            var aboutUs = await _context.AboutUs.ToListAsync();

            var ParentCategoryViewModels = parantCategories?.Select(pc => new IndexViewModel.ParentCategory
            {
                Id = pc.Id,
                Name = pc.Name,
                Description = pc.Description,
                ImagePath = pc.ImagePath,
                Categories = pc.Categories?.Select(c => new IndexViewModel.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagePath = c.ImagePath,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = pc.Name
                }).ToList() ?? new List<IndexViewModel.Category>()
            }).ToList() ?? new List<IndexViewModel.ParentCategory>();

            var footerIconViewModels = footersIcon.Select(s => new IndexViewModel.SocialMedia
            {
                AccountIcon = s.AccountIcon,
                AccountName = s.AccountURLLink,
                AccountURLLink = s.AccountURLLink,
                Is_Active = s.Is_Active


            }).ToList();

            var aboutUsViewModel = aboutUs.Select(s => new IndexViewModel.AboutUs
            {
                Banner_Image = s.Banner_Image,
                Banner_Title = s.Banner_SubTitle,
                Banner_SubTitle = s.Banner_SubTitle,
                Product_Image1 = s.Product_Image1,
                Product1_Title = s.Product1_Title,
                Product1_Description = s.Product1_Description,
                Product_Image2 = s.Product_Image2,
                Product2_Description = s.Product2_Description,
                Product2_Title = s.Product2_Title,
                VideoPath = s.VideoPath,
                VideoTitle = s.VideoTitle,
                Is_Active = s.Is_Active
            }).ToList();

            var footerDescriptionViewModels = footersDescription.Select(f => new IndexViewModel.FooterDescription
            {
                Title = f.Title,
                Description = f.Description,
                Is_Active = f.Is_Active


            }).ToList();

            var footerPagesViewModels = footerPages.Select(p => new IndexViewModel.Page
            {
                CustomePageData = p.CustomePageData,
                CustomePageTittle = p.CustomePageTittle,
                PageCategory = p.PageCategory,
                PageUrl = p.PageUrl,
                Is_Active = p.Is_Active


            }).ToList();
            var viewModel = new IndexViewModel
            {

                ParentCategories = ParentCategoryViewModels,
                Aboutus = aboutUsViewModel

            };

            ViewBag.FooterIcon = footerIconViewModels;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.FooterPage = footerPagesViewModels;


            return View(viewModel);
        }
        public async Task<IActionResult> ContactUs()
        {

            var banners = await _context.Banners.ToListAsync();
            var flash = await _context.FlashMessages.ToListAsync();
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var softbanners = await _context.SoftBanner.ToListAsync();
            var shortbanners = await _context.ShortBanner.ToListAsync();
            var parantCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var recentProducts = await _context.Products.OrderByDescending(rp => rp.Created_Date).FirstOrDefaultAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var rating = await _context.UserRating.ToListAsync();
            var video = await _context.VideoGallery.ToListAsync();
            var stores = await _context.StoreProfiles.ToListAsync();
            var recentBlog = await _context.Blogs.OrderByDescending(rb => rb.Id).FirstOrDefaultAsync();

            var ParentCategoryViewModels = parantCategories?.Select(pc => new IndexViewModel.ParentCategory
            {
                Id = pc.Id,
                Name = pc.Name,
                Description = pc.Description,
                ImagePath = pc.ImagePath,
                Categories = pc.Categories?.Select(c => new IndexViewModel.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagePath = c.ImagePath,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = pc.Name
                }).ToList() ?? new List<IndexViewModel.Category>()
            }).ToList() ?? new List<IndexViewModel.ParentCategory>();

            var footerIconViewModels = footersIcon.Select(s => new IndexViewModel.SocialMedia
            {
                AccountIcon = s.AccountIcon,
                AccountName = s.AccountURLLink,
                AccountURLLink = s.AccountURLLink,
                Is_Active = s.Is_Active


            }).ToList();

            var storeViewModel = stores.Select(store => new IndexViewModel.StoreProfile
            {
                Id = store.Id,
                StoreName = store.StoreName,
                ZipCode = store.ZipCode,
                Description = store.Description,
                Address = store.Address,
                ContactNumber = store.ContactNumber,
                Email = store.Email,
                City = store.City,
                State = store.State,
                ImagePath = store.ImagePath
            }).ToList();

            var footerDescriptionViewModels = footersDescription.Select(f => new IndexViewModel.FooterDescription
            {
                Title = f.Title,
                Description = f.Description,
                Is_Active = f.Is_Active


            }).ToList();

            var footerPagesViewModels = footerPages.Select(p => new IndexViewModel.Page
            {
                CustomePageData = p.CustomePageData,
                CustomePageTittle = p.CustomePageTittle,
                PageCategory = p.PageCategory,
                PageUrl = p.PageUrl,
                Is_Active = p.Is_Active


            }).ToList();
            var viewModel = new IndexViewModel
            {

                ParentCategories = ParentCategoryViewModels,
                StoreProfiles = storeViewModel

            };

            ViewBag.FooterIcon = footerIconViewModels;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.FooterPage = footerPagesViewModels;


            return View(viewModel);
        }
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> FAQ()
        {

            var banners = await _context.Banners.ToListAsync();
            var flash = await _context.FlashMessages.ToListAsync();
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var softbanners = await _context.SoftBanner.ToListAsync();
            var shortbanners = await _context.ShortBanner.ToListAsync();
            var parantCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var recentProducts = await _context.Products.OrderByDescending(rp => rp.Created_Date).FirstOrDefaultAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var rating = await _context.UserRating.ToListAsync();
            var video = await _context.VideoGallery.ToListAsync();
            var recentBlog = await _context.Blogs.OrderByDescending(rb => rb.Id).FirstOrDefaultAsync();
            var faqs = await _context.FAQs.ToListAsync();

            var ParentCategoryViewModels = parantCategories?.Select(pc => new IndexViewModel.ParentCategory
            {
                Id = pc.Id,
                Name = pc.Name,
                Description = pc.Description,
                ImagePath = pc.ImagePath,
                Categories = pc.Categories?.Select(c => new IndexViewModel.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagePath = c.ImagePath,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = pc.Name
                }).ToList() ?? new List<IndexViewModel.Category>()
            }).ToList() ?? new List<IndexViewModel.ParentCategory>();

            var footerIconViewModels = footersIcon.Select(s => new IndexViewModel.SocialMedia
            {
                AccountIcon = s.AccountIcon,
                AccountName = s.AccountURLLink,
                AccountURLLink = s.AccountURLLink,
                Is_Active = s.Is_Active


            }).ToList();

            var FAQsViewModel = faqs.Select(s => new IndexViewModel.FAQ
            {
                Id = s.Id,
                Question = s.Question,
                Answer = s.Answer,
                IsActive = s.IsActive,
                FAQs_Type = s.FAQs_Type


            }).ToList();

            var footerDescriptionViewModels = footersDescription.Select(f => new IndexViewModel.FooterDescription
            {
                Title = f.Title,
                Description = f.Description,
                Is_Active = f.Is_Active


            }).ToList();

            var footerPagesViewModels = footerPages.Select(p => new IndexViewModel.Page
            {
                CustomePageData = p.CustomePageData,
                CustomePageTittle = p.CustomePageTittle,
                PageCategory = p.PageCategory,
                PageUrl = p.PageUrl,
                Is_Active = p.Is_Active


            }).ToList();
            var viewModel = new IndexViewModel
            {

                ParentCategories = ParentCategoryViewModels,
                FAQs = FAQsViewModel,

            };

            ViewBag.FooterIcon = footerIconViewModels;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.FooterPage = footerPagesViewModels;


            return View(viewModel);
        }

        [Route("{pageUrl}")]
        public async Task<IActionResult> DynamicPage(string pageUrl)
        {
            // Fetching the dynamic page data
            var page = await _context.Page.FirstOrDefaultAsync(p => p.PageUrl == pageUrl);
            if (page == null)
            {
                return NotFound();
            }

            // Fetching other related data
            var banners = await _context.Banners.ToListAsync();
            var flash = await _context.FlashMessages.ToListAsync();
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var softbanners = await _context.SoftBanner.ToListAsync();
            var shortbanners = await _context.ShortBanner.ToListAsync();
            var parantCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var recentProducts = await _context.Products.OrderByDescending(rp => rp.Created_Date).FirstOrDefaultAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var rating = await _context.UserRating.ToListAsync();
            var video = await _context.VideoGallery.ToListAsync();
            var recentBlog = await _context.Blogs.OrderByDescending(rb => rb.Id).FirstOrDefaultAsync();
            var faqs = await _context.FAQs.ToListAsync();

            // Mapping ParentCategories
            var ParentCategoryViewModels = parantCategories?.Select(pc => new IndexViewModel.ParentCategory
            {
                Id = pc.Id,
                Name = pc.Name,
                Description = pc.Description,
                ImagePath = pc.ImagePath,
                Categories = pc.Categories?.Select(c => new IndexViewModel.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagePath = c.ImagePath,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = pc.Name
                }).ToList() ?? new List<IndexViewModel.Category>()
            }).ToList() ?? new List<IndexViewModel.ParentCategory>();

            // Mapping SocialMedia Icons
            var footerIconViewModels = footersIcon.Select(s => new IndexViewModel.SocialMedia
            {
                AccountIcon = s.AccountIcon,
                AccountName = s.AccountURLLink,
                AccountURLLink = s.AccountURLLink,
                Is_Active = s.Is_Active
            }).ToList();




            // Mapping FAQ
            var FAQsViewModel = faqs.Select(s => new IndexViewModel.FAQ
            {
                Id = s.Id,
                Question = s.Question,
                Answer = s.Answer,
                IsActive = s.IsActive,
                FAQs_Type = s.FAQs_Type
            }).ToList();

            // Mapping Footer Description
            var footerDescriptionViewModels = footersDescription.Select(f => new IndexViewModel.FooterDescription
            {
                Title = f.Title,
                Description = f.Description,
                Is_Active = f.Is_Active
            }).ToList();

            // Mapping Footer Pages
            var footerPagesViewModels = footerPages.Select(p => new IndexViewModel.Page
            {
                CustomePageData = p.CustomePageData,
                CustomePageTittle = p.CustomePageTittle,
                PageCategory = p.PageCategory,
                PageUrl = p.PageUrl,
                Is_Active = p.Is_Active
            }).ToList();

            // Prepare ViewModel for the Dynamic Page
            var viewModel = new IndexViewModel
            {
                ParentCategories = ParentCategoryViewModels,
                FAQs = FAQsViewModel
            };

            var PageViewModels = new IndexViewModel.Page
            {
                Is_Active = page.Is_Active,
                PageCategory = page.PageCategory,
                PageUrl = page.PageUrl,
                CustomePageData = page.CustomePageData,
                CustomePageTittle = page.CustomePageTittle,
                CustomPageld = page.CustomPageld
            };

            // Adding Footer-related data to ViewBag
            ViewBag.FooterIcon = footerIconViewModels;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.FooterPage = footerPagesViewModels;
            ViewBag.DynamicPages = PageViewModels;

            // Passing the ViewModel and the Page to the view
            return View("DynamicPage", viewModel);
        }




        public async Task<IActionResult> LabResult()
        {

            var banners = await _context.Banners.ToListAsync();
            var flash = await _context.FlashMessages.ToListAsync();
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var softbanners = await _context.SoftBanner.ToListAsync();
            var shortbanners = await _context.ShortBanner.ToListAsync();
            var parantCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var recentProducts = await _context.Products.OrderByDescending(rp => rp.Created_Date).FirstOrDefaultAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var rating = await _context.UserRating.ToListAsync();
            var video = await _context.VideoGallery.ToListAsync();
            var recentBlog = await _context.Blogs.OrderByDescending(rb => rb.Id).FirstOrDefaultAsync();
            var aboutUs = await _context.AboutUs.ToListAsync();
            var lab = await _context.COA.ToListAsync();

            var ParentCategoryViewModels = parantCategories?.Select(pc => new IndexViewModel.ParentCategory
            {
                Id = pc.Id,
                Name = pc.Name,
                Description = pc.Description,
                ImagePath = pc.ImagePath,
                Categories = pc.Categories?.Select(c => new IndexViewModel.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagePath = c.ImagePath,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = pc.Name
                }).ToList() ?? new List<IndexViewModel.Category>()
            }).ToList() ?? new List<IndexViewModel.ParentCategory>();

            var footerIconViewModels = footersIcon.Select(s => new IndexViewModel.SocialMedia
            {
                AccountIcon = s.AccountIcon,
                AccountName = s.AccountURLLink,
                AccountURLLink = s.AccountURLLink,
                Is_Active = s.Is_Active


            }).ToList();

            var aboutUsViewModel = aboutUs.Select(s => new IndexViewModel.AboutUs
            {
                Banner_Image = s.Banner_Image,
                Banner_Title = s.Banner_SubTitle,
                Banner_SubTitle = s.Banner_SubTitle,
                Product_Image1 = s.Product_Image1,
                Product1_Title = s.Product1_Title,
                Product1_Description = s.Product1_Description,
                Product_Image2 = s.Product_Image2,
                Product2_Description = s.Product2_Description,
                Product2_Title = s.Product2_Title,
                VideoPath = s.VideoPath,
                VideoTitle = s.VideoTitle,
                Is_Active = s.Is_Active
            }).ToList();

            var footerDescriptionViewModels = footersDescription.Select(f => new IndexViewModel.FooterDescription
            {
                Title = f.Title,
                Description = f.Description,
                Is_Active = f.Is_Active


            }).ToList();

            var labViewModels = lab.Select(f => new IndexViewModel.COA
            {
                Id = f.Id,
                Product_name = f.Product_name,
                Product_image = f.Product_image,
                Pdf_Path = f.Pdf_Path
            }).ToList();

            var footerPagesViewModels = footerPages.Select(p => new IndexViewModel.Page
            {
                CustomePageData = p.CustomePageData,
                CustomePageTittle = p.CustomePageTittle,
                PageCategory = p.PageCategory,
                PageUrl = p.PageUrl,
                Is_Active = p.Is_Active


            }).ToList();
            var viewModel = new IndexViewModel
            {

                ParentCategories = ParentCategoryViewModels,
                Aboutus = aboutUsViewModel,
                COAs = labViewModels

            };

            ViewBag.FooterIcon = footerIconViewModels;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.FooterPage = footerPagesViewModels;


            return View(viewModel);
        }



        public async Task<IActionResult> Login()
        {
            var banners = await _context.Banners.ToListAsync();
            var flash = await _context.FlashMessages.ToListAsync();
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var softbanners = await _context.SoftBanner.ToListAsync();
            var shortbanners = await _context.ShortBanner.ToListAsync();
            var parantCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var recentProducts = await _context.Products.OrderByDescending(rp => rp.Created_Date).FirstOrDefaultAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var rating = await _context.UserRating.ToListAsync();
            var video = await _context.VideoGallery.ToListAsync();
            var recentBlog = await _context.Blogs.OrderByDescending(rb => rb.Id).FirstOrDefaultAsync();

            var ParentCategoryViewModels = parantCategories?.Select(pc => new IndexViewModel.ParentCategory
            {
                Id = pc.Id,
                Name = pc.Name,
                Description = pc.Description,
                ImagePath = pc.ImagePath,
                Categories = pc.Categories?.Select(c => new IndexViewModel.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagePath = c.ImagePath,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = pc.Name
                }).ToList() ?? new List<IndexViewModel.Category>()
            }).ToList() ?? new List<IndexViewModel.ParentCategory>();

            var footerIconViewModels = footersIcon.Select(s => new IndexViewModel.SocialMedia
            {
                AccountIcon = s.AccountIcon,
                AccountName = s.AccountURLLink,
                AccountURLLink = s.AccountURLLink,
                Is_Active = s.Is_Active


            }).ToList();
            var Blog = blogs.Select(f => new IndexViewModel.Blog
            {
                Id = f.Id,
                ImagePath = f.ImagePath,
                Author = f.Author,
                Content = f.Content,
                Title = f.Title,
                Blog_Category = f.Blog_Category,
                PublishedDate = f.PublishedDate
            }).ToList();

            var footerDescriptionViewModels = footersDescription.Select(f => new IndexViewModel.FooterDescription
            {
                Title = f.Title,
                Description = f.Description,
                Is_Active = f.Is_Active


            }).ToList();

            var footerPagesViewModels = footerPages.Select(p => new IndexViewModel.Page
            {
                CustomePageData = p.CustomePageData,
                CustomePageTittle = p.CustomePageTittle,
                PageCategory = p.PageCategory,
                PageUrl = p.PageUrl,
                Is_Active = p.Is_Active


            }).ToList();
            var viewModel = new IndexViewModel
            {

                ParentCategories = ParentCategoryViewModels,
                Blogs = Blog


            };

            ViewBag.FooterIcon = footerIconViewModels;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.FooterPage = footerPagesViewModels;

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUpUser(CustomerDetails model, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {

                var existingUser = await _context.CustomerDetails
                    .FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "This email is already registered. Please log in.";
                    return RedirectToAction("Login");
                }


                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);


                if (imageFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "User", "ProfilePhoto");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    model.Attachment = "/images/User/ProfilePhoto/" + uniqueFileName;
                }


                model.User_id = Guid.NewGuid();
                model.Password = hashedPassword;
                model.Created_date = DateTime.UtcNow;

                _context.CustomerDetails.Add(model);
                await _context.SaveChangesAsync();


                TempData["SuccessMessage"] = "Your account has been created successfully. Please log in.";


                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var existingUser = await _context.CustomerDetails
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (existingUser == null || !BCrypt.Net.BCrypt.Verify(model.Password, existingUser.Password))
                {
                    TempData["LogErrorMessage"] = "Invalid email or password.";
                    return View(model);
                }

                var profileImageUrl = string.IsNullOrEmpty(existingUser.Attachment) ? "/images/default-profile.png" : existingUser.Attachment;

                var claims = new List<Claim>
                {
                   new Claim(ClaimTypes.NameIdentifier, existingUser.User_id.ToString()),
                   new Claim(ClaimTypes.Email, existingUser.Email),
                   new Claim("ProfileImageUrl", profileImageUrl)

                };


                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };


                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Set session variable (optional, if needed)
                HttpContext.Session.SetString("UserId", existingUser.User_id.ToString());

                TempData["LogSuccessMessage"] = "Login successful!";

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
                return View(model);
            }
        }




        public async Task<IActionResult> UserProfile()
        {
            var banners = await _context.Banners.ToListAsync();
            var flash = await _context.FlashMessages.ToListAsync();
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var softbanners = await _context.SoftBanner.ToListAsync();
            var shortbanners = await _context.ShortBanner.ToListAsync();
            var parantCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var recentProducts = await _context.Products.OrderByDescending(rp => rp.Created_Date).FirstOrDefaultAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var rating = await _context.UserRating.ToListAsync();
            var video = await _context.VideoGallery.ToListAsync();
            var recentBlog = await _context.Blogs.OrderByDescending(rb => rb.Id).FirstOrDefaultAsync();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.CustomerDetails.FirstOrDefault(u => u.User_id.ToString() == userId);

            if (user == null)
            {
                return NotFound();
            }

            var UserProfileViewModels = new IndexViewModel.CustomerDetail
            {
                CustomerID = user.CustomerID,
                Name = user.Name,
                Address = user.Address,
                Attachment = user.Attachment,
                User_id = user.User_id,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                City = user.City,
                Company = user.Company,
                State = user.State,
                ZipCode = user.ZipCode,
                ResellersLicense = user.ResellersLicense,
                DistributorType = user.DistributorType,
                Note = user.Note,
                Created_date = user.Created_date,

            };


            var ParentCategoryViewModels = parantCategories?.Select(pc => new IndexViewModel.ParentCategory
            {
                Id = pc.Id,
                Name = pc.Name,
                Description = pc.Description,
                ImagePath = pc.ImagePath,
                Categories = pc.Categories?.Select(c => new IndexViewModel.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagePath = c.ImagePath,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = pc.Name
                }).ToList() ?? new List<IndexViewModel.Category>()
            }).ToList() ?? new List<IndexViewModel.ParentCategory>();

            var footerIconViewModels = footersIcon.Select(s => new IndexViewModel.SocialMedia
            {
                AccountIcon = s.AccountIcon,
                AccountName = s.AccountURLLink,
                AccountURLLink = s.AccountURLLink,
                Is_Active = s.Is_Active


            }).ToList();
            var Blog = blogs.Select(f => new IndexViewModel.Blog
            {
                Id = f.Id,
                ImagePath = f.ImagePath,
                Author = f.Author,
                Content = f.Content,
                Title = f.Title,
                Blog_Category = f.Blog_Category,
                PublishedDate = f.PublishedDate
            }).ToList();

            var footerDescriptionViewModels = footersDescription.Select(f => new IndexViewModel.FooterDescription
            {
                Title = f.Title,
                Description = f.Description,
                Is_Active = f.Is_Active


            }).ToList();

            var footerPagesViewModels = footerPages.Select(p => new IndexViewModel.Page
            {
                CustomePageData = p.CustomePageData,
                CustomePageTittle = p.CustomePageTittle,
                PageCategory = p.PageCategory,
                PageUrl = p.PageUrl,
                Is_Active = p.Is_Active


            }).ToList();
            var viewModel = new IndexViewModel
            {

                ParentCategories = ParentCategoryViewModels,
                Blogs = Blog


            };

            ViewBag.FooterIcon = footerIconViewModels;
            ViewBag.UserProfile = UserProfileViewModels;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.FooterPage = footerPagesViewModels;

            return View(viewModel);
        }





        public async Task<IActionResult> UserOrders()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    var orders = await _context.OrderDetailsMaster
                        .Where(o => o.User_id.ToString() == userId)
                        .Include(o => o.OrderDetails.CustomerDetails)
                        .Include(o => o.OrderDetails)
                        .Include(o => o.Product)
                        .OrderByDescending(o => o.CreatedDate)
                        .ToListAsync();

                    var orderPagesViewModels = orders
                        .GroupBy(o => o.OrderNo) // Using OrderNo from OrderDetailsMaster
                        .Select(g => new IndexViewModel.OrderDetailsMaster
                        {
                            OrderDetailsMasterId = g.First().OrderDetailsMasterId,
                            OrderId = g.First().OrderId,
                            OrderNo = g.Key, // Directly from OrderDetailsMaster
                            OrderStatus = g.First().OrderDetails.OrderStatus,
                            CreatedDate = g.First().CreatedDate,
                            StoreId = g.First().StoreId,
                            StoreName = g.First().Product?.StoreName ?? "N/A",
                            StoreZipCode = g.First().Product?.StoreZipCode ?? "N/A",
                            TotalPriceWithQuantity = g.Sum(o => o.TotalPriceWithQuantity), // Total price sum
                            Quantity = g.Sum(o => o.Quantity), // Total quantity sum
                            TotalProducts = g.Count() // Total number of products
                        })
                        .ToList();

                    var viewModel = new IndexViewModel
                    {
                        OrderDetailsMasters = orderPagesViewModels
                    };

                    return PartialView("_OrdersPartial", viewModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user orders: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

            return PartialView("_OrdersPartial", new IndexViewModel
            {
                OrderDetailsMasters = new List<IndexViewModel.OrderDetailsMaster>()
            });
        }




        public async Task<IActionResult> UserAddress()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID not found.");
                }

                if (!Guid.TryParse(userId, out Guid userGuid))
                {
                    return BadRequest("Invalid User ID format.");
                }

                var address = await _context.CustomerDetails
                    .FirstOrDefaultAsync(u => u.User_id == userGuid);

                if (address == null)
                {
                    return NotFound("Address not found.");
                }

                // Map the fetched address (CustomerDetails) to the ViewModel
                var UserAddressViewModel = new IndexViewModel.CustomerDetail
                {
                    User_id = address.User_id,
                    City = address.City,
                    State = address.State,
                    ZipCode = address.ZipCode,
                    Created_date = address.Created_date,
                    Address = address.Address
                };

                // Set the ViewBag property
                ViewBag.UserAddressViewModel = UserAddressViewModel;

                return PartialView("_AddressesPartial");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching user address.");
            }
        }


        public async Task<IActionResult> UserAccountDetails()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID not found.");
                }

                if (!Guid.TryParse(userId, out Guid userGuid))
                {
                    return BadRequest("Invalid User ID format.");
                }

                var details = await _context.CustomerDetails
                    .FirstOrDefaultAsync(u => u.User_id == userGuid);

                if (details == null)
                {
                    return NotFound("details not found.");
                }

                // Map the fetched address (CustomerDetails) to the ViewModel
                var UserDetailsViewModel = new IndexViewModel.CustomerDetail
                {
                    CustomerID = details.CustomerID,
                    Name = details.Name,
                    Address = details.Address,
                    Attachment = details.Attachment,
                    User_id = details.User_id,
                    Email = details.Email,
                    Password = details.Password,
                    PhoneNumber = details.PhoneNumber,
                    City = details.City,
                    Company = details.Company,
                    State = details.State,
                    ZipCode = details.ZipCode,
                    ResellersLicense = details.ResellersLicense,
                    DistributorType = details.DistributorType,
                    Note = details.Note,
                    Created_date = details.Created_date,
                };

                // Set the ViewBag property
                ViewBag.UserDetails = UserDetailsViewModel;

                return PartialView("_AccountDetailsPartial");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching user address.");
            }
        }


        [HttpPost]
        public IActionResult EditUserProfileImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Save the file (e.g., to the server or cloud storage)
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Return the new image URL as JSON response
                return Json(new { success = true, newImageUrl = "/images/" + file.FileName });
            }

            return Json(new { success = false });
        }

        public async Task<IActionResult> GetCartDetails()
        {
            try
            {
                List<int> cart = HttpContext.Session.GetObjectFromJson<List<int>>("Cart") ?? new List<int>();



                var checkoutProducts = await _context.Products.Where(p => cart.Contains(p.Id)).ToListAsync();

                decimal totalcartDiscountPrice = checkoutProducts.Sum(p => decimal.TryParse(p.PriceWithDiscount, out var price) ? price : 0);


                var cartProducts = checkoutProducts.Select(p => new IndexViewModel.Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    TittleBadge = p.TittleBadge,
                    PriceWithDiscount = p.PriceWithDiscount,
                    ImagePath = p.ImagePath,
                    SubImage_1 = p.SubImage_1,
                    SubImage_2 = p.SubImage_2,
                    SubImage_3 = p.SubImage_3,
                    SubImage_4 = p.SubImage_4,
                    Flavor = p.Flavor,
                    FeaturedProducts = p.FeaturedProducts,
                    JustArrivedProducts = p.JustArrivedProducts,
                    BadgeIsActive = p.BadgeIsActive,
                    Description = p.Description,
                    Weight = p.Weight,
                    ShortDescription = p.ShortDescription,
                    PricePerPcs = p.PricePerPcs,
                    Created_Date = p.Created_Date,
                    CategoryId = p.CategoryId
                }).ToList();

                var viewModel = new IndexViewModel
                {
                    Products = cartProducts
                };
                ViewBag.TotalCartItem = totalcartDiscountPrice;

                return PartialView("_CartItemsPartial", viewModel);
            }
            catch (Exception ex)
            {
                // Log error

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        public IActionResult GetOrderDetails(int id)
        {
            var orderDetails = _context.OrderDetailsMaster
                .Where(o => o.OrderId == id)
                .Include(o => o.OrderDetails)  // Include Order Details
                .Include(o => o.Product)       // Include Product Details
                .Select(o => new OrderDetailsViewModel
                {
                    OrderId = o.OrderId,
                    OrderNo = o.OrderNo,
                    OrderStatus = o.OrderDetails.OrderStatus,
                    CreatedDate = o.CreatedDate,
                    StoreName = o.Product.StoreName,
                    StoreZipCode = o.Product.StoreZipCode,
                    TotalPriceWithQuantity = o.TotalPriceWithQuantity,
                    Quantity = o.Quantity,
                    ProductName = o.Product.Name ?? "N/A",
                    ProductPrice = o.Product.Price,
                    ProductImage = o.Product.ImagePath

                })
                .ToList();

            if (!orderDetails.Any())
            {
                return NotFound();
            }

            return PartialView("_OrderDetails", orderDetails); // Return a partial view with product details
        }


    }
}
