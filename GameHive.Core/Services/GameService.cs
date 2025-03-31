using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameHive.Core.IServices;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.DataAccess.Repository.Repositories;
using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Http;

namespace GameHive.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRequestRepository _grRepo;
        private readonly IGameRepository _repo;
        private readonly IGameTagRepository _gtrepo;
        private readonly CloudinaryService _cloudinaryService;

        public GameService(IGameRepository repo, IGameTagRepository gtrepo, CloudinaryService cloudinaryService, IGameRequestRepository grRepo)
        {
            _repo = repo;
            _gtrepo = gtrepo;
            _cloudinaryService = cloudinaryService;
            _grRepo = grRepo;
        }
        public async Task DeleteGameAsync(int gameId, string publisherId)
        {
            var game = await _repo.GetByIdAsync(gameId);
            if (game == null || game.PublisherId != publisherId)
                throw new UnauthorizedAccessException("You are not authorized to delete this game.");

            await _repo.DeleteAsync(gameId);
        }

        public async Task DeleteGameTagAsync(int gameId, int tagId)
        {
            await _gtrepo.DeleteAsync(gameId, tagId);
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Game> GetGameWithTagsById(int id)
        {
            return await _repo.GetGameWithTagsById(id);
        }

        public async Task<Game> GetGameByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<List<Game>> GetPublisherGamesAsync(string publisherId)
        {
            return await _repo.GetPublisherGamesAsync(publisherId);
        }

        public async Task UpdateGameAsync(Game game, List<int> Tags, string publisherId)
        {
            var existingGame = await _repo.GetByIdAsync(game.GameId);
            if (existingGame == null || existingGame.PublisherId != publisherId)
                throw new UnauthorizedAccessException("You are not authorized to edit this game.");

            await _repo.UpdateAsync(game);
            await _gtrepo.DeleteByGameIdAsync(game.GameId);

            foreach (var tag in Tags)
            {
                var gameTag = new GameTag { GameId = game.GameId, TagId = tag };
                await _gtrepo.AddAsync(gameTag);
            }
        }

        public async Task<List<string>> GetGameImagesAsync(int id)
        {
            return await _repo.GetGameImagesAsync(id);
        }

        public async Task<bool> RateGameAsync(string userId, int gameId, double ratingValue)
        {
            var game = await _repo.GetByIdAsync(gameId);
            if (game == null)
            {
                return false;
            }
            int storedRating = (int)(ratingValue * 2);

            var existingRating = await _repo.GetUserRatingAsync(userId, gameId);

            if (existingRating != null)
            {
                existingRating.Rating = (RatingEnums)storedRating;
                existingRating.CreatedAt = DateTime.Now;
                await _repo.UpdateUserRatingAsync(existingRating);
            }
            else
            {
                var newRating = new UserRating
                {
                    UserId = userId,
                    GameId = gameId,
                    Rating = (RatingEnums)storedRating,
                    CreatedAt = DateTime.Now
                };
                await _repo.AddRatingAsync(newRating);
            }
            await UpdateGameAverageRatingAsync(gameId);

            return true;
        }

        public async Task UpdateGameAverageRatingAsync(int gameId)
        {
            var ratings = await _repo.GetRatingsByGameIdAsync(gameId);

            if (ratings == null || !ratings.Any())
                return;

            double ratingsum = ratings.Sum(r => (int)r.Rating);
            double averageRating = ratingsum / ratings.Count;
            await _repo.UpdateGameAverageRatingAsync(Math.Round(averageRating), gameId);
        }

        public async Task ProcessOrderAsync(Order order)
        {
            var gameIds = order.OrderDetails.Select(o => o.GameId).ToList();
            var games = await _repo.GetGamesByIdsAsync(gameIds);

            foreach (var game in games)
            {
                var orderItem = order.OrderDetails.First(o => o.GameId == game.GameId);
                game.Orders += orderItem.Quantity;
            }

            foreach (var game in games)
            {
                await _repo.UpdateAsync(game);
            }
        }
    }
}
