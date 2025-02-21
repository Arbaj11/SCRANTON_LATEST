using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scanton.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using static Scanton.Models.IndexViewModel;

namespace Scanton.Controllers
{
    public class ViewCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ViewCartController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [Route("view-orders")]
        public async Task<IActionResult> Index()
        {
            List<int> cart = HttpContext.Session.GetObjectFromJson<List<int>>("Cart") ?? new List<int>();
            var cartProducts = await _context.Products.Where(p => cart.Contains(p.Id)).ToListAsync();
            var banners = await _context.Banners.ToListAsync();
            var flash = await _context.FlashMessages.ToListAsync();
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var softbanners = await _context.SoftBanner.ToListAsync();
            var shortbanners = await _context.ShortBanner.ToListAsync();
            var parentCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var recentProducts = await _context.Products.OrderByDescending(rp => rp.Created_Date).FirstOrDefaultAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var rating = await _context.UserRating.ToListAsync();
            var video = await _context.VideoGallery.ToListAsync();
            var recentBlog = await _context.Blogs.OrderByDescending(rb => rb.Id).FirstOrDefaultAsync();
            var faqs = await _context.FAQs.ToListAsync();

           
            decimal totalPrice = cartProducts.Sum(p => p.Price);
            decimal totalDiscountPrice = cartProducts.Sum(p => decimal.TryParse(p.PriceWithDiscount, out var price) ? price : 0);

            var parentCategoryViewModels = parentCategories?.Select(pc => new IndexViewModel.ParentCategory
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

            var faqsViewModel = faqs.Select(s => new IndexViewModel.FAQ
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


            var viewModel = new IndexViewModel
            {
                ParentCategories = parentCategoryViewModels,
                FAQs = faqsViewModel,
                Products= CartProducts
            };

            ViewBag.FooterIcon = footerIconViewModels;  
            ViewBag.TotalDiscount = totalDiscountPrice;
            ViewBag.CardtProducts = CartProducts;
            ViewBag.TotalPrice = totalPrice;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.FooterPage = footerPagesViewModels;


            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddToCart(int product_id)
        {
            if (product_id <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid product ID." });
            }
            List<int> cart = HttpContext.Session.GetObjectFromJson<List<int>>("Cart") ?? new List<int>();
            if (!cart.Contains(product_id))
            {
                cart.Add(product_id);
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return Ok(new { success = true, message = "Product added to cart!", cartCount = cart.Count });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int product_id)
        {
            if (product_id <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid product ID." });
            }

            // Retrieve the current cart from session
            List<int> cart = HttpContext.Session.GetObjectFromJson<List<int>>("Cart") ?? new List<int>();

            // Check if the product is in the cart
            if (cart.Contains(product_id))
            {
                cart.Remove(product_id);
                HttpContext.Session.SetObjectAsJson("Cart", cart);

                return Ok(new { success = true, message = "Product removed from cart!", cartCount = cart.Count });
            }

            return NotFound(new { success = false, message = "Product not found in cart." });
        }

        [HttpPost]
        public IActionResult ApplyCoupon(string couponName, decimal totalPrice)
        {
            try
            {
                if (string.IsNullOrEmpty(couponName))
                {
                    return Json(new { success = false, message = "Coupon code is required." });
                }

                var coupon = _context.Coupons.FirstOrDefault(c => c.Code == couponName);

                if (coupon == null)
                {
                    return Json(new { success = false, message = "Invalid coupon code." });
                }
                else
                {
                    if(coupon.ExpiryDate >= DateTime.UtcNow )
                    {
                        decimal discountPercentage = coupon.Discount > 0 ? coupon.Discount : 0;
                        decimal discountAmount = (totalPrice * discountPercentage) / 100;
                        decimal newTotal = totalPrice - discountAmount;

                        return Json(new
                        {
                            success = true,
                            message = "Coupon applied successfully!",
                            discountAmount = discountAmount,
                            newTotal = newTotal,
                            couponId = coupon.Id
                        });
                    }
                    else
                    {
                        return Json(new { success = false, message = "This coupon has expired." });
                    }
                    
                }

                
               
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while applying the coupon.", error = ex.Message });
            }
        }



        [Route("check-out-products")]
        public async Task<IActionResult> CheckOutProducts(decimal totalPrice, decimal totalGstPrice)
        {
            List<int> cart = HttpContext.Session.GetObjectFromJson<List<int>>("Cart") ?? new List<int>();
           
            var banners = await _context.Banners.ToListAsync();
            var flash = await _context.FlashMessages.ToListAsync();
            var footersIcon = await _context.SocialMedia.ToListAsync();
            var footersDescription = await _context.FooterDescription.ToListAsync();
            var footerPages = await _context.Page.ToListAsync();
            var softbanners = await _context.SoftBanner.ToListAsync();
            var shortbanners = await _context.ShortBanner.ToListAsync();
            var parentCategories = await _context.ParentCategories.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var recentProducts = await _context.Products.OrderByDescending(rp => rp.Created_Date).FirstOrDefaultAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var rating = await _context.UserRating.ToListAsync();
            var video = await _context.VideoGallery.ToListAsync();
            var recentBlog = await _context.Blogs.OrderByDescending(rb => rb.Id).FirstOrDefaultAsync();
            var faqs = await _context.FAQs.ToListAsync();


            var checkoutProducts = await _context.Products.Where(p => cart.Contains(p.Id)).ToListAsync();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                var user = _context.CustomerDetails.FirstOrDefault(u => u.User_id.ToString() == userId);
                var CheckOutUserDetails = new IndexViewModel.CustomerDetail
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

                ViewBag.CheckOutUserDetails = CheckOutUserDetails;
            }

            var subtotal = totalPrice + 50;

            var parentCategoryViewModels = parentCategories?.Select(pc => new IndexViewModel.ParentCategory
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

            var faqsViewModel = faqs.Select(s => new IndexViewModel.FAQ
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

            var CartProducts = checkoutProducts.Select(p => new IndexViewModel.Product
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


            var viewModel = new IndexViewModel
            {
                ParentCategories = parentCategoryViewModels,
                FAQs = faqsViewModel,
                Products = CartProducts
            };

            ViewBag.FooterIcon = footerIconViewModels;
            ViewBag.FooterDescription = footerDescriptionViewModels;
            ViewBag.TotalPrice = totalPrice;
            ViewBag.SubTotal = subtotal;
            ViewBag.FooterPage = footerPagesViewModels;


            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> ProcessPayment(decimal TotalAmount, decimal Discount)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                List<int> cart = HttpContext.Session.GetObjectFromJson<List<int>>("Cart") ?? new List<int>();
                var checkoutProducts = await _context.Products.Where(p => cart.Contains(p.Id)).ToListAsync();
                var shoppingCart = HttpContext.Session.GetObjectFromJson<List<CartItemViewModel>>("ShoppingCart") ?? new List<CartItemViewModel>();

                Guid? User_ID = null;

                
                if (Guid.TryParse(userId, out Guid parsedGuid))
                {
                    User_ID = parsedGuid; 
                }

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User not found or is not authenticated.");
                }

                var user = await _context.CustomerDetails.FirstOrDefaultAsync(u => u.User_id.ToString() == userId);

                if (user == null)
                {
                    throw new Exception("User details not found.");
                }

                var customerDetails = await _context.CustomerDetails
                                                  .Where(c => c.CustomerID == user.CustomerID)
                                                  .ToListAsync();

                if (customerDetails.Count == 0)
                {
                    throw new Exception("No customer details found for the given CustomerID.");
                }

                bool isUserOrderExists = await _context.OrderDetails.AnyAsync();

                if (isUserOrderExists)
                {
                    var IsUserAvailable = await _context.OrderDetails.FirstOrDefaultAsync(o => o.CustomerId == user.CustomerID);

                    if (IsUserAvailable == null)
                    {
                        var CustomerOrderDetails = new OrderDetails
                        {
                            OrderDate = DateTime.UtcNow,
                            CustomerId = user.CustomerID,
                            Comment = "Pending",
                            TotalAmount = TotalAmount,
                            Discount = Discount,
                            ShippingCharges = 50,
                            OrderStatus = "Pending",
                            PaymentMode = "Credit Card",
                            OrderTime = DateTime.UtcNow.TimeOfDay,
                            UpdateBy = "ByStore",
                            OrderNo = 1
                        };

                        _context.OrderDetails.Add(CustomerOrderDetails);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (IsUserAvailable.OrderNo == null)
                        {
                            IsUserAvailable.OrderNo = 1;
                        }

                        if (cart.Count > 0)
                        {
                            IsUserAvailable.OrderNo += 1;
                            IsUserAvailable.TotalAmount = IsUserAvailable.TotalAmount + TotalAmount;
                            _context.OrderDetails.Update(IsUserAvailable);
                            await _context.SaveChangesAsync();
                        }
                    }

                    foreach (var product in checkoutProducts)
                    {
                        var cartItem = shoppingCart.FirstOrDefault(p => p.ProductId == product.Id);
                        int quantity = cartItem?.Quantity ?? 0; // Get quantity from session cart, default to 0 if not found

                       
                            var orderDetailMaster = new OrderDetailsMaster
                            {
                                OrderId = IsUserAvailable.OrderId, // Assuming OrderId is the primary key of OrderDetails
                                ProductId = product.Id,
                                User_id=User_ID,
                                ProductPrice = product.Price,
                                TotalPriceWithQuantity = product.Price * quantity,
                                StoreId=product.StoreProfileId,
                                StoreName=product.StoreName,
                                CreatedDate = DateTime.UtcNow
                            };

                        if (quantity > 0)
                        {
                            orderDetailMaster.Quantity = quantity;
                            orderDetailMaster.TotalPriceWithQuantity = product.Price * quantity;
                        }
                        else
                        {
                            orderDetailMaster.Quantity = 1;
                            orderDetailMaster.TotalPriceWithQuantity = product.Price;
                        }

                        _context.OrderDetailsMaster.Add(orderDetailMaster);
                        
                        await _context.SaveChangesAsync(); // Save order details in OrderDetailsMaster
                    }
                    
                }
                else
                {
                    var CustomerOrderDetails = new OrderDetails
                    {
                        OrderDate = DateTime.UtcNow,
                        CustomerId = user.CustomerID,
                        Comment = "Pending",
                        TotalAmount = TotalAmount,
                        Discount = Discount,
                        ShippingCharges = 50,
                        OrderStatus = "Pending",
                        PaymentMode = "Credit Card",
                        OrderTime = DateTime.UtcNow.TimeOfDay,
                        UpdateBy = "ByStore",
                        OrderNo = 1
                    };

                    _context.OrderDetails.Add(CustomerOrderDetails);
                    await _context.SaveChangesAsync();

                    foreach (var product in checkoutProducts)
                    {
                        var cartItem = shoppingCart.FirstOrDefault(p => p.ProductId == product.Id);
                        int quantity = cartItem?.Quantity ?? 0; // Get quantity from session cart, default to 0 if not found

                       
                            var orderDetailMaster = new OrderDetailsMaster
                            {
                                OrderId = CustomerOrderDetails.OrderId, // Assuming OrderId is the primary key of OrderDetails
                                ProductId = product.Id,
                                ProductPrice = product.Price,
                                User_id = User_ID,
                                StoreId = product.StoreProfileId,
                                StoreName = product.StoreName,
                                CreatedDate = DateTime.UtcNow
                            };
                        if (quantity > 0)
                        {
                            orderDetailMaster.Quantity = quantity;
                            orderDetailMaster.TotalPriceWithQuantity = product.Price * quantity;
                        }
                        else
                        {
                            orderDetailMaster.Quantity = 1;
                            orderDetailMaster.TotalPriceWithQuantity = product.Price;
                        }

                        _context.OrderDetailsMaster.Add(orderDetailMaster);
                        
                        await _context.SaveChangesAsync();
                    }
                    // Save order details in OrderDetailsMaster
                }

                HttpContext.Session.Remove("Cart");

                // Set TempData to show success modal
                TempData["PaymentSuccess"] = true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the payment.", ex);
            }

            return RedirectToAction("CheckOutProducts");
        }



        [HttpPost]
        public IActionResult UpdateQuantity([FromBody] CartItemViewModel model)
        {
            if (model == null || model.ProductId <= 0)
                return BadRequest("Invalid data.");

            // Get or initialize session cart (use "ShoppingCart" as the session key)
            var shoppingCart = HttpContext.Session.GetObjectFromJson<List<CartItemViewModel>>("ShoppingCart") ?? new List<CartItemViewModel>();

            // Check if the product already exists in the cart
            var existingProduct = shoppingCart.FirstOrDefault(p => p.ProductId == model.ProductId);
            if (existingProduct != null)
            {
                existingProduct.Quantity = model.Quantity;
            }
            else
            {
                shoppingCart.Add(new CartItemViewModel { ProductId = model.ProductId, Quantity = model.Quantity });
            }

            // Save the updated cart back to session with the new name "ShoppingCart"
            HttpContext.Session.SetObjectAsJson("ShoppingCart", shoppingCart);

            return Json(new { success = true, message = "Quantity updated successfully." });
        }


    }
}
