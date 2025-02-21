using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scanton.Models;
using System.Reflection.Emit;
using static Scanton.Models.IndexViewModel;

namespace Scanton.Controllers
{
    public class SingleProductController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SingleProductController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }


        [Route("product-details/{product_id}")]
        public async Task<IActionResult> ProductDetails(int product_id, string zipCode)
        {
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var products = await _context.Products.ToListAsync(); ;
            var parantCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            var productDetails = await _context.Products.FirstOrDefaultAsync(p => p.Id == product_id);
            
            var filteredProducts = products.Where(p => p.CategoryId == productDetails.CategoryId).ToList();


            var footerIconViewModels = footersIcon.Select(s => new IndexViewModel.SocialMedia
            {
                AccountIcon = s.AccountIcon,
                AccountName = s.AccountURLLink,
                AccountURLLink = s.AccountURLLink,
                Is_Active = s.Is_Active


            }).ToList();
            var footerPagesViewModels = footerPages.Select(p => new IndexViewModel.Page
            {
                CustomePageData = p.CustomePageData,
                CustomePageTittle = p.CustomePageTittle,
                PageCategory = p.PageCategory,
                PageUrl = p.PageUrl,
                Is_Active = p.Is_Active
            }).ToList();

            var footerDescriptionViewModels = footersDescription.Select(f => new IndexViewModel.FooterDescription
            {
                Title = f.Title,
                Description = f.Description,
                Is_Active = f.Is_Active
            }).ToList();

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
            }).ToList();

            var singleProductViewModel = new IndexViewModel.Product
            {
                Id = productDetails.Id,
                Name = productDetails.Name,
                ImagePath = productDetails.ImagePath,
                TittleBadge = productDetails.TittleBadge,
                PriceWithDiscount = productDetails.PriceWithDiscount,
                Price = productDetails.Price,
                Flavor = productDetails.Flavor,
                SubImage_1 = productDetails.SubImage_1,
                SubImage_2 = productDetails.SubImage_2,
                SubImage_3 = productDetails.SubImage_3,
                SubImage_4 = productDetails.SubImage_4,
                BadgeIsActive = productDetails.BadgeIsActive,
                Weight = productDetails.Weight,
                PricePerPcs = productDetails.PricePerPcs,
                FeaturedProducts = productDetails.FeaturedProducts,
                CategoryId = productDetails.CategoryId,
                Description = productDetails.Description,
                ShortDescription = productDetails.ShortDescription,
                // Assuming categories is already fetched, and you want the category name
                CategoryName = categories.FirstOrDefault(c => c.Id == productDetails.CategoryId)?.Name ?? "Unknown"
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

            var productCatgeoryViewModels = filteredProducts.Select(p => new IndexViewModel.Product
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
            }).ToList();


            var viewModel = new IndexViewModel
            {
                Products = productCatgeoryViewModels,
                ParentCategories = ParentCategoryViewModels
            };

            ViewBag.ProductID = singleProductViewModel.Id;
            ViewBag.FooterIcon = footerIconViewModels;
            ViewBag.FooterPage = footerPagesViewModels;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.SingleProduct = singleProductViewModel;

            return View(viewModel);
        }


      
        public async Task<IActionResult> ShopDetailsProduct(string zipCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(zipCode))
                {
                    
                    var shops = await _context.StoreProfiles
                        .Where(s => s.ZipCode == zipCode)
                        .ToListAsync();

                    if (shops.Any()) 
                    {
                        var storeViewModel = shops.Select(p => new IndexViewModel.StoreProfile
                        {
                            Id = p.Id,
                            StoreName = p.StoreName,
                            ZipCode = zipCode,
                            Description = p.Description,
                            Address = p.Address,
                            ContactNumber = p.ContactNumber,
                            Email = p.Email,
                            City=p.City,
                            State=p.State

                        }).ToList();

                        var viewModel = new IndexViewModel
                        {
                             StoreProfiles= storeViewModel,
                            
                        };

                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return PartialView("_ShopDetailsProduct", viewModel);
                        }

                        return View(viewModel);
                    }
                    else
                    {
                        return Json(new { success = false, message = "No shops found for the given zip code." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Invalid zip code." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return Json(new { success = false, message = "An error occurred while retrieving shop details." });
            }
        }



        public async Task<IActionResult> AvailableProductsByStore(int store_id, int pageNumber = 1, int pageSize = 10)
        {
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var products = await _context.Products.ToListAsync(); ;
            var parantCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            var store = await _context.StoreProfiles.FirstOrDefaultAsync(s => s.Id == store_id);
            var storeProducts = await _context.Products.Where(p => p.StoreProfileId == store_id).ToListAsync();


            var totalItems = storeProducts.Count;
            
            // Paginate the store products
            var paginatedProducts = storeProducts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            


            var storeDetails = new IndexViewModel.StoreProfile
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
                ImagePath=store.ImagePath


            };

            var storeProductsViewModel = storeProducts.Select(s => new IndexViewModel.Product
            {
                Id = s.Id,
                Name = s.Name,
                ImagePath = s.ImagePath,
                TittleBadge = s.TittleBadge,
                PriceWithDiscount = s.PriceWithDiscount,
                Price = s.Price,
                Flavor = s.Flavor,
                SubImage_1 = s.SubImage_1,
                SubImage_2 = s.SubImage_2,
                SubImage_3 = s.SubImage_3,
                SubImage_4 = s.SubImage_4,
                BadgeIsActive = s.BadgeIsActive,
                Weight = s.Weight,
                PricePerPcs = s.PricePerPcs,
                FeaturedProducts = s.FeaturedProducts,
                CategoryId = s.CategoryId,
                Description = s.Description,
                ShortDescription = s.ShortDescription,
                // Assuming categories is already fetched, and you want the category name
                CategoryName = categories.FirstOrDefault(c => c.Id == s.CategoryId)?.Name ?? "Unknown"

            }).ToList();



            var footerIconViewModels = footersIcon.Select(s => new IndexViewModel.SocialMedia
            {
                AccountIcon = s.AccountIcon,
                AccountName = s.AccountURLLink,
                AccountURLLink = s.AccountURLLink,
                Is_Active = s.Is_Active


            }).ToList();
            var footerPagesViewModels = footerPages.Select(p => new IndexViewModel.Page
            {
                CustomePageData = p.CustomePageData,
                CustomePageTittle = p.CustomePageTittle,
                PageCategory = p.PageCategory,
                PageUrl = p.PageUrl,
                Is_Active = p.Is_Active
            }).ToList();

            var footerDescriptionViewModels = footersDescription.Select(f => new IndexViewModel.FooterDescription
            {
                Title = f.Title,
                Description = f.Description,
                Is_Active = f.Is_Active
            }).ToList();

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

            


            var viewModel = new IndexViewModel
            {
               
                ParentCategories = ParentCategoryViewModels,
                Products=storeProductsViewModel,
                
            };
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.StoreProfile = storeDetails;
            ViewBag.FooterIcon = footerIconViewModels;
            ViewBag.FooterPage = footerPagesViewModels;
            ViewBag.FooterDescription = footerDescriptionViewModels;
           

            return View(viewModel);
        }


       
    }
}
