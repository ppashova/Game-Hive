﻿using GameHive.Core.IServices;
using GameHive.DataAccess.Repository; // Make sure this is added if not already
using GameHive.Core.Services;
using GameHive.Models;
using GameHive.Models.enums;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameHive.DataAccess.Repository.IRepositories;

namespace GameHive.Tests.Services
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _orderRepoMock;
        private IOrderService _orderService;
        private IGameService _gameService;

        [SetUp]
        public void SetUp()
        {
            _orderRepoMock = new Mock<IOrderRepository>();
            _orderService = new OrderService(_orderRepoMock.Object, _gameService); // 
        }

        [Test]
        public async Task CreateOrderAsync_CreatesOrderAndDetails()
        {
            // Arrange
            string userId = "user123";
            string firstName = "John";
            string lastName = "Doe";
            string email = "john.doe@example.com";
            decimal totalPrice = 99.99m;
            List<int> gameIds = new List<int> { 1, 2, 3 };
            List<int> quantities = new List<int> { 1, 2, 1 };

            Order capturedOrder = null;
            List<OrderDetail> capturedOrderDetails = null;

            _orderRepoMock.Setup(r => r.AddOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(order => capturedOrder = order)
                .Returns(Task.CompletedTask);

            _orderRepoMock.Setup(r => r.AddOrderDetailsAsync(It.IsAny<List<OrderDetail>>()))
                .Callback<List<OrderDetail>>(details => capturedOrderDetails = details)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.CreateOrderAsync(
                userId, firstName, lastName, email, totalPrice, gameIds, quantities);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.FirstName, Is.EqualTo(firstName));
            Assert.That(result.LastName, Is.EqualTo(lastName));
            Assert.That(result.Email, Is.EqualTo(email));
            Assert.That(result.TotalPrice, Is.EqualTo(totalPrice));
            Assert.That(result.Status, Is.EqualTo(OrderStatusEnums.Pending));

            _orderRepoMock.Verify(r => r.AddOrderAsync(It.IsAny<Order>()), Times.Once);
            _orderRepoMock.Verify(r => r.AddOrderDetailsAsync(It.IsAny<List<OrderDetail>>()), Times.Once);

            Assert.That(capturedOrderDetails, Is.Not.Null);
            Assert.That(capturedOrderDetails.Count, Is.EqualTo(3));

            for (int i = 0; i < gameIds.Count; i++)
            {
                var detail = capturedOrderDetails.FirstOrDefault(d => d.GameId == gameIds[i]);
                Assert.That(detail, Is.Not.Null);
                Assert.That(detail.OrderId, Is.EqualTo(capturedOrder.Id));
                Assert.That(detail.Quantity, Is.EqualTo(quantities[i]));
            }
        }

        [Test]
        public async Task GetOrderByIdAsync_ReturnsOrder()
        {
            var orderId = Guid.NewGuid();
            var expectedOrder = new Order { Id = orderId, UserId = "user1" };

            _orderRepoMock.Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync(expectedOrder);

            var result = await _orderService.GetOrderByIdAsync(orderId);

            Assert.That(result, Is.EqualTo(expectedOrder));
            _orderRepoMock.Verify(r => r.GetOrderByIdAsync(orderId), Times.Once);
        }

        [Test]
        public async Task GetUserOrdersAsync_ReturnsUserOrders()
        {
            string userId = "user123";
            var expectedOrders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), UserId = userId },
                new Order { Id = Guid.NewGuid(), UserId = userId }
            };

            _orderRepoMock.Setup(r => r.GetUserOrdersAsync(userId))
                .ReturnsAsync(expectedOrders);

            var result = await _orderService.GetUserOrdersAsync(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result, Is.EquivalentTo(expectedOrders));
            _orderRepoMock.Verify(r => r.GetUserOrdersAsync(userId), Times.Once);
        }

        [Test]
        public async Task GetAllOrdersAsync_ReturnsAllOrders()
        {
            var expectedOrders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), UserId = "user1" },
                new Order { Id = Guid.NewGuid(), UserId = "user2" },
                new Order { Id = Guid.NewGuid(), UserId = "user3" }
            };

            _orderRepoMock.Setup(r => r.GetAllOrdersAsync())
                .ReturnsAsync(expectedOrders);

            var result = await _orderService.GetAllOrdersAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result, Is.EquivalentTo(expectedOrders));
            _orderRepoMock.Verify(r => r.GetAllOrdersAsync(), Times.Once);
        }

        [Test]
        public async Task GetTotalPriceAsync_ReturnsTotalPrice()
        {
            var orderId = Guid.NewGuid();
            decimal expectedPrice = 149.99m;

            _orderRepoMock.Setup(r => r.GetTotalPriceAsync(orderId))
                .ReturnsAsync(expectedPrice);

            var result = await _orderService.GetTotalPriceAsync(orderId);

            Assert.That(result, Is.EqualTo(expectedPrice));
            _orderRepoMock.Verify(r => r.GetTotalPriceAsync(orderId), Times.Once);
        }
    }
}
