// GameHive.Controllers/ShoppingCartController.cs
using GameHive.Core.IServices;
using GameHive.Core.Services;
using GameHive.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameHive.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            var cartItems = await _shoppingCartService.GetCartItemsAsync();
            var cartTotal = await _shoppingCartService.GetCartTotalAsync();

            ViewBag.CartTotal = cartTotal;
            return View(cartItems); 
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }

            await _shoppingCartService.AddToCartAsync(productId, quantity);
            return RedirectToAction("Index");
        }

        // POST: ShoppingCart/RemoveFromCart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            await _shoppingCartService.RemoveFromCartAsync(productId);
            var cartItems = await _shoppingCartService.GetCartItemsAsync();
            decimal cartTotal = cartItems.Sum(item => item.Quantity * item.Game.Price);

            // Optionally update the cart count and other session variables if necessary
            ViewBag.CartTotal = cartTotal;

            return RedirectToAction("Index");
        }



        // POST: ShoppingCart/ClearCart
        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            await _shoppingCartService.ClearCartAsync();
            return RedirectToAction("Index"); 
        }
    }
}