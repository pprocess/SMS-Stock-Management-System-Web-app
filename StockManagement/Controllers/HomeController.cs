using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using System;
using System.Linq;

namespace StockManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Get all products from last 10 days
            var recentProducts = _context.Products
                .Where(p => p.LastPurchasedDate >= DateTime.Now.AddDays(-10))
                .OrderByDescending(p => p.LastPurchasedDate)
                .ToList();

            // Get top 10 popular products regardless of date
            var popularProducts = _context.Products
                .OrderByDescending(p => p.PurchaseCount)
                .Take(10)
                .ToList();

            // Combine and deduplicate
            var allProducts = recentProducts
                .Union(popularProducts)
                .Distinct()
                .OrderByDescending(p => p.LastPurchasedDate)
                .ThenByDescending(p => p.PurchaseCount)
                .ToList();

            ViewBag.IsLoggedIn = User.Identity?.IsAuthenticated ?? false;
            ViewBag.IsManager = User.IsInRole("Manager");
            ViewBag.Username = User.Identity?.Name;

            return View(allProducts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}