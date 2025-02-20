// GameHive.DataAccess/ShoppingCartRepository.cs
using GameHive.DataAccess;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace GameHive.DataAccess.Repository
{


    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(string cartId, int productId, int quantity)
        {
            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.CartId == cartId && c.GameId == productId);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    CartId = cartId,
                    GameId = productId,
                    Quantity = quantity
                };
                _context.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string cartId, int productId)
        {
            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.CartId == cartId && c.GameId == productId);

            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Cart>> GetCartItemsAsync(string cartId)
        {
            return await _context.Carts
                .Where(c => c.CartId == cartId)
                .Include(c => c.Game)
                .ToListAsync();
        }

        public async Task<decimal> GetCartTotalAsync(string cartId)
        {
            return await _context.Carts
                .Where(c => c.CartId == cartId)
                .SumAsync(c => c.Quantity * c.Game.Price);
        }

        public async Task ClearCartAsync(string cartId)
        {
            var cartItems = await _context.Carts
                .Where(c => c.CartId == cartId)
                .ToListAsync();

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}
