using GameHive.Core.IServices;
using GameHive.Core.Services;
using GameHive.Models;
using GameHive.Models.Order_View_Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace GameHive.Controllers
{
    [Authorize(Roles = "User")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _shoppingCartService;

        public OrdersController(IOrderService orderService, IShoppingCartService shoppingCartService)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItems = await _shoppingCartService.GetCartItemsAsync();
            var totalPrice = await _shoppingCartService.GetCartTotalAsync();

            var checkoutModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                TotalPrice = totalPrice,
                FirstName = "",
                LastName = "",
                Email = User.FindFirstValue(ClaimTypes.Email) ?? ""
            };

            return View(checkoutModel);

        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model, List<int> gameIds)
        {
            if (!ModelState.IsValid)
            {
                model.CartItems = await _shoppingCartService.GetCartItemsAsync();
                model.TotalPrice = await _shoppingCartService.GetCartTotalAsync();
                return View(model);
            }

            string userId = User.Identity.GetUserId();

            var orderDetails = gameIds.Select(gameId => new OrderDetail { GameId = gameId }).ToList();

            var order = await _orderService.CreateOrderAsync(userId, model.Email, model.TotalPrice, orderDetails);

            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }


    }

}

