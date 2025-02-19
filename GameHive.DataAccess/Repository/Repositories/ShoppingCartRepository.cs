using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;

namespace GameHive.DataAccess.Repository.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        public async Task AddToCartAsync(Cart cartItem)
        {
            _context.Carts.Add(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task EmptyCartAsync(string CartId)
        {
            var cartItems = _context.Carts.Where(c => c.CartId == CartId);
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCartItemAsync(string cartId, int gameId)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId && c.GameId == gameId);
        }

        public async Task<List<Cart>> GetCartItemsAsync(string cartId)
        {
            return await _context.Carts.Where(c => c.CartId == cartId).Include(c => c.Game).ToListAsync();
        }

        public async Task RemoveFromCartAsync(Cart cartItem)
        {
            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Cart cartItem)
        {
            _context.Carts.Update(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}
