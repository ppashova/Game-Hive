using GameHive.Core.IServices;
using GameHive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameHive.Controllers
{
    [Authorize(Roles = "User")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: Orders/Checkout
        public IActionResult Checkout()
        {
            return View();
        }

        // POST: Orders/Checkout
        [HttpPost]
        public async Task<IActionResult> Checkout(Order model, List<int> gameIds)
        {
            if (gameIds == null || !gameIds.Any())
            {
                ModelState.AddModelError("", "Your cart is empty!");
                return View(model);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string email = User.FindFirstValue(ClaimTypes.Email);

            var orderDetails = gameIds.Select(gameId => new OrderDetail { GameId = gameId }).ToList();

            var order = await _orderService.CreateOrderAsync(userId, email, model.TotalPrice, orderDetails);

            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }
    }

}

