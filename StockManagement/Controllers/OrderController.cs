using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;
using System;
using System.Linq;
using System.Security.Claims;

namespace StockManagement.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // User Order History
        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == int.Parse(userId))
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // Manager Order Management
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public IActionResult ManageOrders()
        {
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Where(o => o.Status == "Pending")
                .OrderBy(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int orderId, string status)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null) return NotFound();

            if (status == "Cancelled")
            {
                foreach (var item in order.OrderItems)
                {
                    var product = _context.Products.Find(item.ProductId);
                    product.Quantity += item.Quantity;
                }
            }

            order.Status = status;
            _context.SaveChanges();

            TempData["Message"] = $"Order #{orderId} updated to {status}";
            return RedirectToAction("ManageOrders");
        }

        [HttpGet]
        public IActionResult CreateOrder(int productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                TempData["Error"] = "Product not found!";
                return RedirectToAction("Index", "Home");
            }

            if (product.Quantity <= 0)
            {
                TempData["Error"] = "This product is out of stock!";
                return RedirectToAction("Index", "Home");
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(int productId, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("Index", "Home");
            }

            if (quantity <= 0)
            {
                TempData["Error"] = "Quantity must be at least 1!";
                return RedirectToAction("CreateOrder", new { productId });
            }

            var product = _context.Products.Find(productId);
            if (product == null)
            {
                TempData["Error"] = "Product not found!";
                return RedirectToAction("Index", "Home");
            }

            if (product.Quantity < quantity)
            {
                TempData["Error"] = $"Only {product.Quantity} available in stock!";
                return RedirectToAction("CreateOrder", new { productId });
            }

            try
            {
                var order = new Order
                {
                    UserId = int.Parse(userId),
                    OrderDate = DateTime.Now,
                    Status = "Pending"
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = product.Price
                };

                _context.OrderItems.Add(orderItem);

                product.Quantity -= quantity;
                product.PurchaseCount += quantity;
                product.LastPurchasedDate = DateTime.Now;

                _context.SaveChanges();

                TempData["Success"] = $"Order placed successfully! ({quantity}x {product.Name})";
                return RedirectToAction("Index", "Order");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error placing order: {ex.Message}";
                return RedirectToAction("CreateOrder", new { productId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id && o.UserId == int.Parse(userId) && o.Status == "Pending");

            if (order == null)
            {
                return NotFound();
            }

            try
            {
                foreach (var item in order.OrderItems)
                {
                    var product = _context.Products.Find(item.ProductId);
                    if (product != null)
                    {
                        product.Quantity += item.Quantity;
                        product.PurchaseCount -= item.Quantity;
                    }
                }

                order.Status = "Cancelled";
                _context.SaveChanges();

                TempData["Message"] = "Order cancelled successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error cancelling order: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}