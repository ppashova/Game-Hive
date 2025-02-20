// GameHive.Core/Services/ShoppingCartService.cs
using GameHive.Core.IServices;
using GameHive.DataAccess;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameHive.Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartService(
            IShoppingCartRepository shoppingCartRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<string> GetCartIdAsync()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string? cartId = session.GetString("CartId");

            if (cartId == null)
            {
                cartId = Guid.NewGuid().ToString();
                session.SetString("CartId", cartId);
            }

            return cartId;
        }
        public async Task AddToCartAsync(int productId, int quantity)
        {
            string cartId = await GetCartIdAsync();
            await _shoppingCartRepository.AddToCartAsync(cartId, productId, quantity);
        }

        public async Task RemoveFromCartAsync(int productId)
        {
            string cartId = await GetCartIdAsync();
            await _shoppingCartRepository.RemoveFromCartAsync(cartId, productId);
        }

        public async Task<List<Cart>> GetCartItemsAsync()
        {
            string cartId = await GetCartIdAsync();
            return await _shoppingCartRepository.GetCartItemsAsync(cartId);
        }

        public async Task<decimal> GetCartTotalAsync()
        {
            string cartId = await GetCartIdAsync();
            return await _shoppingCartRepository.GetCartTotalAsync(cartId);
        }

        public async Task ClearCartAsync()
        {
            string cartId = await GetCartIdAsync();
            await _shoppingCartRepository.ClearCartAsync(cartId);
        }


        public async Task<decimal> GetCartTotal()
        {
            string cartId = await GetCartIdAsync();
            return await _shoppingCartRepository.GetCartTotalAsync(cartId);
        }


        public async Task ClearCart()
        {
            string cartId = await GetCartIdAsync();
            await _shoppingCartRepository.ClearCartAsync(cartId);
        }
    }
}