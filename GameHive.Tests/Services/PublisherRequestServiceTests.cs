using GameHive.Core.Services;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GameHive.Tests.Services
{
    [TestFixture]
    public class PublisherRequestServiceTests
    {
        private Mock<IRepository<PublisherRequest>> _mockRepository;
        private PublisherRequestService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IRepository<PublisherRequest>>();
            _service = new PublisherRequestService(_mockRepository.Object);
        }

        [Test]
        public async Task AddAsync_CallsRepositoryAddAsync()
        {
            // Arrange
            var publisherRequest = new PublisherRequest { RequestId = 1, UserId = "user1" };

            // Act
            await _service.AddAsync(publisherRequest);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(publisherRequest), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_CallsRepositoryDeleteAsync()
        {
            // Arrange
            int id = 1;

            // Act
            await _service.DeleteAsync(id);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Test]
        public async Task FindAsync_CallsRepositoryFindAsync_ReturnsResult()
        {
            // Arrange
            Expression<Func<PublisherRequest, bool>> filter = pr => pr.UserId == "user1";
            var expectedResult = new List<PublisherRequest>
            {
                new PublisherRequest { RequestId = 1, UserId = "user1" }
            };
            _mockRepository.Setup(r => r.FindAsync(filter)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.FindAsync(filter);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
            _mockRepository.Verify(r => r.FindAsync(filter), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_CallsRepositoryGetAllAsync_ReturnsResult()
        {
            // Arrange
            var expectedResult = new List<PublisherRequest>
            {
                new PublisherRequest { RequestId = 1, UserId = "user1" },
                new PublisherRequest { RequestId = 2, UserId = "user2" }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetAsync_CallsRepositoryGetAsync_ReturnsResult()
        {
            // Arrange
            int id = 1;
            var expectedResult = new PublisherRequest { RequestId = id, UserId = "user1" };
            _mockRepository.Setup(r => r.GetAsync(id)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.GetAsync(id);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
            _mockRepository.Verify(r => r.GetAsync(id), Times.Once);
        }

        [Test]
        public async Task GetRequestByUserIdAsync_CallsRepositoryGetByUserIdAsync_ReturnsResult()
        {
            // Arrange
            string userId = "user1";
            var expectedResult = new PublisherRequest { RequestId = 1, UserId = userId };
            _mockRepository.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.GetRequestByUserIdAsync(userId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
            _mockRepository.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_CallsRepositoryUpdateAsync()
        {
            // Arrange
            var publisherRequest = new PublisherRequest { RequestId = 1, UserId = "user1" };

            // Act
            await _service.UpdateAsync(publisherRequest);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(publisherRequest), Times.Once);
        }

        [Test]
        public async Task GetRequestByUserIdAsync_ReturnsNull_WhenUserNotFound()
        {
            // Arrange
            string userId = "nonexistent";
            _mockRepository.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync((PublisherRequest)null);

            // Act
            var result = await _service.GetRequestByUserIdAsync(userId);

            // Assert
            Assert.That(result, Is.Null);
            _mockRepository.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
        }
    }
}