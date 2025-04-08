using GameHive.Core.IServices;
using GameHive.Core.Services;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using GameHive.Models.enums;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameHive.Tests.Services
{
    [TestFixture]
    public class GameServiceTests
    {
        private Mock<IGameRepository> _gameRepoMock;
        private IGameService _gameService;
        private Mock<IGameTagRepository> _gameTagRepoMock;
        private Mock<IGameRequestRepository> _grRepositoryMock;
        private readonly CloudinaryService _cloudinaryServiceMock;

        [SetUp]
        public void SetUp()
        {
            _gameRepoMock = new Mock<IGameRepository>();
            _gameTagRepoMock = new Mock<IGameTagRepository>();
            _grRepositoryMock = new Mock<IGameRequestRepository>();
            _gameService = new GameService(_gameRepoMock.Object, _gameTagRepoMock.Object, _cloudinaryServiceMock, _grRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllGamesAsync_ReturnsAllGames()
        {
            var games = new List<Game> {
                new Game { GameId = 1 },
                new Game { GameId = 2 }
            };
            _gameRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(games);

            var result = await _gameService.GetAllGamesAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetGameByIdAsync_ValidId_ReturnsGame()
        {
            var game = new Game { GameId = 1 };
            _gameRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(game);

            var result = await _gameService.GetGameByIdAsync(1);

            Assert.That(result, Is.EqualTo(game));
        }

        [Test]
        public async Task DeleteGameAsync_ValidPublisher_DeletesGame()
        {
            var game = new Game { GameId = 1, PublisherId = "abc" };
            _gameRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(game);

            await _gameService.DeleteGameAsync(1, "abc");

            _gameRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Test]
        public void DeleteGameAsync_InvalidPublisher_ThrowsException()
        {
            var game = new Game { GameId = 1, PublisherId = "expected" };
            _gameRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(game);

            Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            {
                await _gameService.DeleteGameAsync(1, "wrong");
            });
        }

        [Test]
        public async Task DeleteGameTagAsync_DeletesEntry()
        {
            await _gameService.DeleteGameTagAsync(1, 2);

            _gameTagRepoMock.Verify(r => r.DeleteAsync(1, 2), Times.Once);
        }

        [Test]
        public async Task UpdateGameAsync_ValidData_UpdatesGame()
        {
            var game = new Game { GameId = 1, PublisherId = "pub" };
            _gameRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(game);

            await _gameService.UpdateGameAsync(game, new List<int> { 1, 2 }, "pub");

            _gameRepoMock.Verify(r => r.UpdateAsync(game), Times.Once);
        }

        [Test]
        public void UpdateGameAsync_InvalidPublisher_ThrowsException()
        {
            var game = new Game { GameId = 1, PublisherId = "expected" };
            _gameRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(game);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _gameService.UpdateGameAsync(game, new List<int>(), "wrong"));
        }

        [Test]
        public async Task GetGameWithTagsById_ReturnsGameWithTags()
        {
            var game = new Game { GameId = 1, GameTags = new List<GameTag>() };
            _gameRepoMock.Setup(r => r.GetGameWithTagsById(1)).ReturnsAsync(game);

            var result = await _gameService.GetGameWithTagsById(1);

            Assert.That(result.GameId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetGameImagesAsync_ReturnsListOfImageUrls()
        {
            var imageUrls = new List<string> { "url", "url2" };

            _gameRepoMock.Setup(r => r.GetGameImagesAsync(1)).ReturnsAsync(imageUrls);
            var result = await _gameService.GetGameImagesAsync(1);

            Assert.That(result.Count, Is.EqualTo(2));
            CollectionAssert.AreEqual(imageUrls, result);
        }



        [Test]
        public async Task RateGameAsync_AddsRating()
        {
            // Setup
            string userId = "user1";
            int gameId = 1;
            double rating = 4.5;

            // Mock GetByIdAsync to return a game
            var game = new Game { GameId = gameId };
            _gameRepoMock.Setup(r => r.GetByIdAsync(gameId)).ReturnsAsync(game);

            // Mock GetUserRatingAsync to return null (simulating no existing rating)
            _gameRepoMock.Setup(r => r.GetUserRatingAsync(userId, gameId)).ReturnsAsync((UserRating)null);

            // Mock AddRatingAsync to return true
            _gameRepoMock.Setup(r => r.AddRatingAsync(It.IsAny<UserRating>())).Returns(Task.FromResult(true));

            // Mock UpdateGameAverageRatingAsync which is called at the end of RateGameAsync
            // We don't need to return anything specific for void methods
            _gameRepoMock.Setup(r => r.UpdateGameAverageRatingAsync(It.IsAny<double>(), gameId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _gameService.RateGameAsync(userId, gameId, rating);

            // Assert
            Assert.That(result, Is.True);

            // Verify that AddRatingAsync was called
            _gameRepoMock.Verify(r => r.AddRatingAsync(It.Is<UserRating>(ur =>
                ur.UserId == userId &&
                ur.GameId == gameId &&
                ur.Rating == (RatingEnums)9)), Times.Once); // 4.5 * 2 = 9
        }
        [Test]
        public async Task UpdateGameAverageRatingAsync_UpdatesRating()
        {
            // Setup
            int gameId = 1;

            // Mock GetRatingsByGameIdAsync to return a list of ratings
            var ratings = new List<UserRating>
    {
        new UserRating { Rating = RatingEnums.TwoStars },  // 4
        new UserRating { Rating = RatingEnums.ThreeStars }    // 6
        // Average would be (4 + 6) / 2 = 5 after rounding
    };

            _gameRepoMock.Setup(r => r.GetRatingsByGameIdAsync(gameId))
                .ReturnsAsync(ratings);

            _gameRepoMock.Setup(r => r.UpdateGameAverageRatingAsync(5, gameId))
                .Returns(Task.CompletedTask);

            // Act
            await _gameService.UpdateGameAverageRatingAsync(gameId);

            // Assert - verify with the exact expected value of 5 (after Math.Round)
            _gameRepoMock.Verify(r => r.UpdateGameAverageRatingAsync(5, gameId), Times.Once);
        }

        [Test]
        public async Task ProcessOrderAsync_ProcessesOrder()
        {
            // Create a valid Order with OrderDetails
            var gameId = 1; // Use a consistent game ID
            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderDetails = new List<OrderDetail>
        {
            new OrderDetail
            {
                GameId = gameId,
                Quantity = 2
            }
        }
            };

            // Set up the mock repository to return a game when GetGamesByIdsAsync is called
            var game = new Game { GameId = gameId, Orders = 0 };
            _gameRepoMock.Setup(r => r.GetGamesByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<Game> { game });

            // Execute the method
            await _gameService.ProcessOrderAsync(order);

            // Verify that UpdateAsync was called with the updated game
            _gameRepoMock.Verify(r => r.UpdateAsync(It.Is<Game>(g =>
                g.GameId == gameId && g.Orders == 2)), Times.Once);
        }

        [Test]
        public async Task GetPublisherGamesAsync_ReturnsList()
        {
            var games = new List<Game> { new Game { PublisherId = "pub1" } };
            _gameRepoMock.Setup(r => r.GetPublisherGamesAsync("pub1")).ReturnsAsync(games);

            var result = await _gameService.GetPublisherGamesAsync("pub1");

            Assert.That(result.Count, Is.EqualTo(1));
        }
    }
}
