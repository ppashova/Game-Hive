using CloudinaryDotNet;
using GameHive.Core.IServices;
using GameHive.Core.Services;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHive.Tests.Services
{
    [TestFixture]
    public class GameRequestServiceTests
    {
        private Mock<IGameRequestRepository> _requestRepoMock;
        private Mock<IGameRepository> _gameRepoMock;
        private Mock<IGameTagRepository> _gameTagRepoMock;
        private CloudinaryService _cloudinaryService; // Not mocked since it's an external service
        private IGameRequestService _gameRequestService;

        [SetUp]
        public void SetUp()
        {
            _requestRepoMock = new Mock<IGameRequestRepository>();
            _gameRepoMock = new Mock<IGameRepository>();
            _gameTagRepoMock = new Mock<IGameTagRepository>();

            // Skip tests that depend on CloudinaryService
            // We'll create tests that don't require file uploads

            _gameRequestService = new GameRequestService(
                _requestRepoMock.Object,
                _gameRepoMock.Object,
                _gameTagRepoMock.Object,
                _cloudinaryService); // This will be null, but we won't call methods that use it
        }

        // Skip the AddGameRequestAsync test since it relies heavily on CloudinaryService

        [Test]
        public async Task GetPendingRequestsAsync_ReturnsRequests()
        {
            // Arrange
            var pendingRequests = new List<GameRequest>
            {
                new GameRequest { Id = 1, Status = RequestEnums.Pending },
                new GameRequest { Id = 2, Status = RequestEnums.Pending }
            };

            _requestRepoMock.Setup(r => r.GetPendingRequestsAsync())
                .ReturnsAsync(pendingRequests);

            // Act
            var result = await _gameRequestService.GetPendingRequestsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Is.EquivalentTo(pendingRequests));
            _requestRepoMock.Verify(r => r.GetPendingRequestsAsync(), Times.Once);
        }

        [Test]
        public async Task ApproveGameRequestAsync_WithAddRequestType_AddsGame()
        {
            // Arrange
            int requestId = 1;
            var gameRequest = new GameRequest
            {
                Id = requestId,
                Title = "New Game",
                BriefDescription = "Brief description",
                FullDescription = "Full description",
                Price = 29.99m,
                GameIconUrl = "icon-url", // Already set (no upload needed)
                GameHeaderUrl = "header-url", // Already set (no upload needed)
                PublisherId = "publisher1",
                RequestType = RequestTypeEnums.Add,
                Status = RequestEnums.Pending,
                Tags = new List<RequestTag>
                {
                    new RequestTag { TagId = 1, RequestId = requestId },
                    new RequestTag { TagId = 2, RequestId = requestId }
                },
                Images = new List<RequestImage>
                {
                    new RequestImage { ImageUrl = "image1-url", RequestId = requestId },
                    new RequestImage { ImageUrl = "image2-url", RequestId = requestId }
                }
            };

            _requestRepoMock.Setup(r => r.GetByIdAsync(requestId))
                .ReturnsAsync(gameRequest);

            Game capturedGame = null;
            _gameRepoMock.Setup(r => r.AddAsync(It.IsAny<Game>()))
                .Callback<Game>(g => capturedGame = g)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _gameRequestService.ApproveGameRequestAsync(requestId);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(gameRequest.Status, Is.EqualTo(RequestEnums.Approved));

            // Verify game was added with correct properties
            _gameRepoMock.Verify(r => r.AddAsync(It.IsAny<Game>()), Times.Once);
            Assert.That(capturedGame, Is.Not.Null);
            Assert.That(capturedGame.Name, Is.EqualTo(gameRequest.Title));
            Assert.That(capturedGame.BriefDescription, Is.EqualTo(gameRequest.BriefDescription));
            Assert.That(capturedGame.FullDescription, Is.EqualTo(gameRequest.FullDescription));
            Assert.That(capturedGame.Price, Is.EqualTo(gameRequest.Price));
            Assert.That(capturedGame.GameIconUrl, Is.EqualTo(gameRequest.GameIconUrl));
            Assert.That(capturedGame.GameHeaderUrl, Is.EqualTo(gameRequest.GameHeaderUrl));
            Assert.That(capturedGame.PublisherId, Is.EqualTo(gameRequest.PublisherId));

            // Verify tags were added
            foreach (var tag in gameRequest.Tags)
            {
                _gameTagRepoMock.Verify(r => r.AddAsync(
                    It.Is<GameTag>(gt => gt.GameId == capturedGame.GameId && gt.TagId == tag.TagId)),
                    Times.Once);
            }

            // Verify images were added
            foreach (var image in gameRequest.Images)
            {
                _gameRepoMock.Verify(r => r.AddGameImageWithUrl(capturedGame.GameId, image.ImageUrl),
                    Times.Once);
            }

            // Verify request was updated
            _requestRepoMock.Verify(r => r.UpdateRequestAsync(gameRequest), Times.Once);
        }

        [Test]
        public async Task ApproveGameRequestAsync_WithEditRequestType_UpdatesGame()
        {
            // Arrange
            int requestId = 1;
            int gameId = 10;

            var gameRequest = new GameRequest
            {
                Id = requestId,
                GameId = gameId,
                Title = "Updated Game",
                BriefDescription = "Updated brief",
                FullDescription = "Updated full description",
                Price = 39.99m,
                GameIconUrl = "new-icon-url", // Already set (no upload needed)
                GameHeaderUrl = "new-header-url", // Already set (no upload needed)
                PublisherId = "publisher1",
                RequestType = RequestTypeEnums.Edit,
                Status = RequestEnums.Pending,
                Tags = new List<RequestTag>
                {
                    new RequestTag { TagId = 3, RequestId = requestId },
                    new RequestTag { TagId = 4, RequestId = requestId }
                }
            };

            var existingGame = new Game
            {
                GameId = gameId,
                Name = "Original Game",
                BriefDescription = "Original brief",
                FullDescription = "Original full description",
                Price = 19.99m,
                GameIconUrl = "original-icon-url",
                GameHeaderUrl = "original-header-url",
                PublisherId = "publisher1"
            };

            var imageUrls = new List<string> { "new-image1-url", "new-image2-url" };

            _requestRepoMock.Setup(r => r.GetByIdAsync(requestId))
                .ReturnsAsync(gameRequest);
            _gameRepoMock.Setup(r => r.GetByIdAsync(gameId))
                .ReturnsAsync(existingGame);
            _requestRepoMock.Setup(r => r.GetRequestImagesAsync(requestId))
                .ReturnsAsync(imageUrls);

            // Act
            var result = await _gameRequestService.ApproveGameRequestAsync(requestId);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(gameRequest.Status, Is.EqualTo(RequestEnums.Approved));

            // Verify game was updated with correct properties
            Assert.That(existingGame.Name, Is.EqualTo(gameRequest.Title));
            Assert.That(existingGame.BriefDescription, Is.EqualTo(gameRequest.BriefDescription));
            Assert.That(existingGame.FullDescription, Is.EqualTo(gameRequest.FullDescription));
            Assert.That(existingGame.Price, Is.EqualTo(gameRequest.Price));
            Assert.That(existingGame.GameIconUrl, Is.EqualTo(gameRequest.GameIconUrl));
            Assert.That(existingGame.GameHeaderUrl, Is.EqualTo(gameRequest.GameHeaderUrl));

            _gameRepoMock.Verify(r => r.UpdateAsync(existingGame), Times.Once);

            // Verify old tags were deleted and new tags added
            _gameTagRepoMock.Verify(r => r.DeleteByGameIdAsync(gameId), Times.Once);
            foreach (var tag in gameRequest.Tags)
            {
                _gameTagRepoMock.Verify(r => r.AddAsync(
                    It.Is<GameTag>(gt => gt.GameId == gameId && gt.TagId == tag.TagId)),
                    Times.Once);
            }

            // Verify old images were deleted and new images added
            _gameRepoMock.Verify(r => r.DeleteAllGameImagesAsync(gameId), Times.Once);
            foreach (var imageUrl in imageUrls)
            {
                _gameRepoMock.Verify(r => r.AddGameImageWithUrl(gameId, imageUrl), Times.Once);
            }

            // Verify request was updated
            _requestRepoMock.Verify(r => r.UpdateRequestAsync(gameRequest), Times.Once);
        }

        [Test]
        public async Task ApproveGameRequestAsync_WithNonExistentRequest_ReturnsFalse()
        {
            // Arrange
            int requestId = 999;
            _requestRepoMock.Setup(r => r.GetByIdAsync(requestId))
                .ReturnsAsync((GameRequest)null);

            // Act
            var result = await _gameRequestService.ApproveGameRequestAsync(requestId);

            // Assert
            Assert.That(result, Is.False);
            _requestRepoMock.Verify(r => r.UpdateRequestAsync(It.IsAny<GameRequest>()), Times.Never);
        }

        [Test]
        public async Task RejectGameRequestAsync_RejectsRequest()
        {
            // Arrange
            int requestId = 1;
            var gameRequest = new GameRequest
            {
                Id = requestId,
                Status = RequestEnums.Pending
            };

            _requestRepoMock.Setup(r => r.GetByIdAsync(requestId))
                .ReturnsAsync(gameRequest);

            // Act
            await _gameRequestService.RejectGameRequestAsync(requestId);

            // Assert
            Assert.That(gameRequest.Status, Is.EqualTo(RequestEnums.Rejected));
            _requestRepoMock.Verify(r => r.UpdateRequestAsync(gameRequest), Times.Once);
        }

        [Test]
        public async Task GetGameRequestByIdAsync_ReturnsRequest()
        {
            // Arrange
            int requestId = 1;
            var expectedRequest = new GameRequest { Id = requestId };

            _requestRepoMock.Setup(r => r.GetByIdAsync(requestId))
                .ReturnsAsync(expectedRequest);

            // Act
            var result = await _gameRequestService.GetGameRequestByIdAsync(requestId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedRequest));
            _requestRepoMock.Verify(r => r.GetByIdAsync(requestId), Times.Once);
        }

        [Test]
        public async Task GetPublisherRequestsById_ReturnsPublisherRequests()
        {
            // Arrange
            string publisherId = "publisher1";
            var expectedRequests = new List<GameRequest>
            {
                new GameRequest { Id = 1, PublisherId = publisherId },
                new GameRequest { Id = 2, PublisherId = publisherId }
            };

            _requestRepoMock.Setup(r => r.GetPublisherRequestsById(publisherId))
                .ReturnsAsync(expectedRequests);

            // Act
            var result = await _gameRequestService.GetPublisherRequestsById(publisherId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedRequests));
            _requestRepoMock.Verify(r => r.GetPublisherRequestsById(publisherId), Times.Once);
        }

        [Test]
        public async Task AddExistingImageToRequestAsync_AddsImage()
        {
            // Arrange
            int requestId = 1;
            string imageUrl = "existing-image-url";

            // Act
            await _gameRequestService.AddExistingImageToRequestAsync(requestId, imageUrl);

            // Assert
            _requestRepoMock.Verify(r => r.AddRequestImagesAsync(requestId, imageUrl), Times.Once);
        }
    }
}