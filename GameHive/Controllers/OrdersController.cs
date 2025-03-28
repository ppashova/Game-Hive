using GameHive.Core.IServices;
using GameHive.Core.Services;
using GameHive.Models;
using GameHive.Models.Order_View_Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Stripe.Climate;
using System.Security.Claims;

namespace GameHive.Controllers
{
    [Authorize(Roles = "User")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IGameService _gameService;

        public OrdersController(IOrderService orderService, IShoppingCartService shoppingCartService, IGameService gameService)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
            _gameService = gameService;
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
            //if (!ModelState.IsValid)
            //{
            //    model.CartItems = await _shoppingCartService.GetCartItemsAsync();
            //    model.TotalPrice = await _shoppingCartService.GetCartTotalAsync();
            //    return View(model);
            //}
            model.TotalPrice = await _shoppingCartService.GetCartTotalAsync();
            string userId = User.Identity.GetUserId();
            var order = await _orderService.CreateOrderAsync(userId,model.FirstName,model.LastName, model.Email, model.TotalPrice, gameIds);
            await _gameService.ProcessOrderAsync(order);
            return RedirectToAction("OrderConfirmation");
        }
        public IActionResult OrderConfirmation()
        {
            return View();
        }

    }

}

