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
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IProductService productService, UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            
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
    }
}
