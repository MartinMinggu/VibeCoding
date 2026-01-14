// Controllers/HomeController.cs
using ECommerceApp.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IAnnouncementService _announcementService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            IProductService productService, 
            IAnnouncementService announcementService,
            UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _announcementService = announcementService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            
            // Get active announcements for display
            var announcements = await _announcementService.GetActiveAnnouncementsAsync();
            ViewBag.Announcements = announcements;
            
            if (user != null && user.IsSeller)
            {
                var products = await _productService.GetTopSellingProductsAsync(8, user.Id);
                ViewBag.HomeTitle = "Your Best Sellers";
                return View(products);
            }
            else
            {
                var products = await _productService.GetTopSellingProductsAsync(8);
                ViewBag.HomeTitle = "Trending Products";
                return View(products);
            }
        }

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        public async Task<IActionResult> AnnouncementDetail(int id)
        {
            var announcement = await _announcementService.GetAnnouncementByIdAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            return View(announcement);
        }

        public async Task<IActionResult> Announcements()
        {
            var announcements = await _announcementService.GetActiveAnnouncementsAsync();
            return View(announcements);
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.DefaultCookieName,
                Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.MakeCookieValue(new Microsoft.AspNetCore.Localization.RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
