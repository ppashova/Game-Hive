using GameHive.Core.Services;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameHive.Tests.Services
{
    [TestFixture]
    public class SupportRequestServiceTests
    {
        private Mock<ISupportRequestRepository> _mockRepo;
        private SupportRequestService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<ISupportRequestRepository>();
            _service = new SupportRequestService(_mockRepo.Object);
        }

        [Test]
        public async Task CreateRequestAsync_Should_Call_AddAsync()
        {
            // Arrange
            var request = new SupportRequest { RequestId = new Guid(), Subject = "Help", ProblemDescription = "I need help" };

            // Act
            await _service.CreateRequestAsync(request);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(request), Times.Once);
        }

        [Test]
        public async Task GetAllRequestsAsync_Should_Return_All_Requests()
        {
            // Arrange
            var expected = new List<SupportRequest>
            {
                new SupportRequest { RequestId = Guid.NewGuid(), Subject = "Request 1" },
                new SupportRequest { RequestId = Guid.NewGuid(), Subject = "Request 2" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllRequestsAsync();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2)); // Fix: Use Assert.That with Is.EqualTo
            Assert.That(result[0].Subject, Is.EqualTo("Request 1")); // Fix: Use Assert.That with Is.EqualTo
        }

        [Test]
        public async Task GetRequestById_Should_Return_Correct_Request()
        {
            // Arrange
            var requestId = "1"; // Fix: Use string instead of Guid
            var expected = new SupportRequest
            {
                RequestId = Guid.NewGuid(), // Keep this as Guid since the model uses it
                Subject = "My Request"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(requestId)).ReturnsAsync(expected); // Fix: Pass string requestId

            // Act
            var result = await _service.GetRequestById(requestId); // Fix: Pass string requestId

            // Assert
            Assert.That(expected.Subject, Is.EqualTo(result.Subject));
        }
    }
}
