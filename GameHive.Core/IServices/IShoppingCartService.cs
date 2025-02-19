using System.Collections.Generic;
using System.Threading.Tasks;
using GameHive.Models;

namespace GameHive.Core.IServices
{
    public interface IShoppingCartService
    {
        string GetCartId();
        Task AddToCartAsync(Game game);
        Task RemoveFromCartAsync(int gameId);
        Task<List<Cart>> GetCartItemsAsync();
        Task EmptyCartAsync();
    }
}