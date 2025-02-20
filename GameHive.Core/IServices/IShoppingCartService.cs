using System.Collections.Generic;
using System.Threading.Tasks;
using GameHive.Models;
using System.Web;
using System.Runtime.CompilerServices;

namespace GameHive.Core.IServices
{
    public interface IShoppingCartService
    {
        Task AddToCartAsync(int productId, int quantity);
        Task RemoveFromCartAsync(int productId);
        Task<List<Cart>> GetCartItemsAsync();
        Task<decimal> GetCartTotalAsync();
        Task ClearCartAsync();
    }

}