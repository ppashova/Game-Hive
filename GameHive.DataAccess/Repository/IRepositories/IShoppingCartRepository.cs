using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameHive.Models;

namespace GameHive.DataAccess.Repository.IRepositories
{
    public interface IShoppingCartRepository
    {
        Task AddToCartAsync(string cartId, int productId, int quantity);
        Task RemoveFromCartAsync(string cartId, int productId);
        Task<List<Cart>> GetCartItemsAsync(string cartId);
        Task<decimal> GetCartTotalAsync(string cartId);
        Task ClearCartAsync(string cartId);
    }
}
