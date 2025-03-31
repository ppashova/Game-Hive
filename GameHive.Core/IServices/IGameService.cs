using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GameHive.Models;
using Microsoft.AspNetCore.Http;

namespace GameHive.Core.IServices
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllGamesAsync();
        Task<Game> GetGameByIdAsync(int id);
        Task DeleteGameAsync(int id, string publisherid);
        Task DeleteGameTagAsync(int gameId, int tagId);
        Task UpdateGameAsync(Game game, List<int> tagIds, string publisherId);
        Task<Game> GetGameWithTagsById(int id);
        Task<List<string>> GetGameImagesAsync(int id);
        Task<bool> RateGameAsync(string userId, int gameId, double ratingValue);
        Task UpdateGameAverageRatingAsync(int gameId);
        Task ProcessOrderAsync(Order order);
        Task<List<Game>> GetPublisherGamesAsync(string publisherId);
    }
}
