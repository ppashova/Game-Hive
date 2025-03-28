using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Repository.IRepositories
{
    public interface IGameRepository
    {
        Task AddAsync(Game game);
        Task DeleteAsync(int id);
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game> GetGameWithTagsById(int id);
        Task<Game> GetByIdAsync(int id);
        Task UpdateAsync(Game game);
        Task<List<Game>> GetPendingGamesAsync();
        Task<int> GetRequestCountAsync();
        Task AddGameImagesAsync(GameImage gameImage);
        Task<List<string>> GetGameImagesAsync(int id);
        Task<List<UserRating>> GetRatingsByGameIdAsync(int gameId);
        Task AddRatingAsync(UserRating rating);
        Task UpdateGameAverageRatingAsync(double newRating, int gameId);
        Task<UserRating?> GetUserRatingAsync(string userId, int gameId);
        Task UpdateUserRatingAsync(UserRating rating);
        Task DeleteRatingAsync(UserRating rating);
        Task<List<Game>> GetGamesByIdsAsync(List<int> GameIds);
    }
}
