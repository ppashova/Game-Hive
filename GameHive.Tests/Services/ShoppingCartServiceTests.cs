using GameHive.Core.IServices;
using GameHive.Core.Services;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GameHive.Tests.Services
{
    [TestFixture]
    public class ShoppingCartServiceTests
    {
        private Mock<IShoppingCartRepository> _shoppingCartRepoMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<HttpContext> _httpContextMock;
        private TestSession _session;
        private IShoppingCartService _shoppingCartService;
        private string _cartId;

        // Custom TestSession class that implements ISession
        private class TestSession : ISession
        {
            private readonly Dictionary<string, byte[]> _store = new Dictionary<string, byte[]>();

            public string Id => "TestSession";
            public bool IsAvailable => true;
            public IEnumerable<string> Keys => _store.Keys;

            public void Clear()
            {
                _store.Clear();
            }

            public Task CommitAsync(CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task LoadAsync(CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public void Remove(string key)
            {
                _store.Remove(key);
            }

            public void Set(string key, byte[] value)
            {
                _store[key] = value;
            }

            public bool TryGetValue(string key, out byte[] value)
            {
                return _store.TryGetValue(key, out value);
            }
        }

        [SetUp]
        public void SetUp()
        {
            _cartId = Guid.NewGuid().ToString();

            // Setup session with custom implementation
            _session = new TestSession();

            // Pre-populate session with cart ID if needed
            if (_cartId != null)
            {
                _session.Set("CartId", System.Text.Encoding.UTF8.GetBytes(_cartId));
            }

            // Setup mocks
            _shoppingCartRepoMock = new Mock<IShoppingCartRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextMock = new Mock<HttpContext>();

            // Setup HttpContext to use our custom session
            _httpContextMock.Setup(c => c.Session).Returns(_session);
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(_httpContextMock.Object);

            // Create service
            _shoppingCartService = new ShoppingCartService(
                _shoppingCartRepoMock.Object,
                _httpContextAccessorMock.Object);
        }

        [Test]
        public async Task AddToCartAsync_CallsRepository()
        {
            // Arrange
            int productId = 1;
            int quantity = 2;

            // Act
            await _shoppingCartService.AddToCartAsync(productId, quantity);

            // Assert
            _shoppingCartRepoMock.Verify(r => r.AddToCartAsync(_cartId, productId, quantity), Times.Once);
        }

        [Test]
        public async Task RemoveFromCartAsync_CallsRepository()
        {
            // Arrange
            int productId = 1;

            // Act
            await _shoppingCartService.RemoveFromCartAsync(productId);

            // Assert
            _shoppingCartRepoMock.Verify(r => r.RemoveFromCartAsync(_cartId, productId), Times.Once);
        }

        [Test]
        public async Task GetCartItemsAsync_ReturnsCartItems()
        {
            // Arrange
            var expectedCartItems = new List<Cart>
            {
                new Cart { CartId = _cartId, GameId = 1, Quantity = 2 },
                new Cart { CartId = _cartId, GameId = 2, Quantity = 1 }
            };

            _shoppingCartRepoMock.Setup(r => r.GetCartItemsAsync(_cartId))
                .ReturnsAsync(expectedCartItems);

            // Act
            var result = await _shoppingCartService.GetCartItemsAsync();

            // Assert
            Assert.That(result, Is.EqualTo(expectedCartItems));
            _shoppingCartRepoMock.Verify(r => r.GetCartItemsAsync(_cartId), Times.Once);
        }

        [Test]
        public async Task GetCartTotalAsync_ReturnsTotal()
        {
            // Arrange
            decimal expectedTotal = 99.99m;

            _shoppingCartRepoMock.Setup(r => r.GetCartTotalAsync(_cartId))
                .ReturnsAsync(expectedTotal);

            // Act
            var result = await _shoppingCartService.GetCartTotalAsync();

            // Assert
            Assert.That(result, Is.EqualTo(expectedTotal));
            _shoppingCartRepoMock.Verify(r => r.GetCartTotalAsync(_cartId), Times.Once);
        }

        [Test]
        public async Task ClearCartAsync_CallsRepository()
        {
            // Act
            await _shoppingCartService.ClearCartAsync();

            // Assert
            _shoppingCartRepoMock.Verify(r => r.ClearCartAsync(_cartId), Times.Once);
        }

        [Test]
        public async Task GetCartTotal_ReturnsTotal()
        {
            // Arrange
            decimal expectedTotal = 149.99m;

            _shoppingCartRepoMock.Setup(r => r.GetCartTotalAsync(_cartId))
                .ReturnsAsync(expectedTotal);

            // Act
            var result = await _shoppingCartService.GetCartTotalAsync();

            // Assert
            Assert.That(result, Is.EqualTo(expectedTotal));
            _shoppingCartRepoMock.Verify(r => r.GetCartTotalAsync(_cartId), Times.Once);
        }

        [Test]
        public async Task ClearCart_CallsRepository()
        {
            // Act
            await _shoppingCartService.ClearCartAsync();

            // Assert
            _shoppingCartRepoMock.Verify(r => r.ClearCartAsync(_cartId), Times.Once);
        }

        [Test]
        public void UpdateCartItemCount_SetsSessionValue()
        {
            // Arrange
            int count = 5;

            // Act
            _shoppingCartService.UpdateCartItemCount(count);

            // Assert
            byte[] countBytes;
            _session.TryGetValue("CartItemCount", out countBytes);

            Assert.That(countBytes, Is.Not.Null);
            Assert.That(BitConverter.ToInt32(countBytes, 0), Is.EqualTo(count));
        }

        [Test]
        public async Task GetCartIdAsync_CreatesNewCartIdWhenNotExists()
        {
            // Setup with a clean session (no CartId)
            SetUp();
            _session = new TestSession();  // Clean session
            _httpContextMock.Setup(c => c.Session).Returns(_session);

            // Act
            // Use private method through a public one
            await _shoppingCartService.GetCartItemsAsync();

            // Assert
            byte[] cartIdBytes;
            bool hasCartId = _session.TryGetValue("CartId", out cartIdBytes);

            Assert.That(hasCartId, Is.True);
            Assert.That(cartIdBytes, Is.Not.Null);

            string cartId = System.Text.Encoding.UTF8.GetString(cartIdBytes);
            Assert.That(cartId, Is.Not.Empty);

            // Verify Guid format
            Assert.That(Guid.TryParse(cartId, out _), Is.True);
        }
    }
}