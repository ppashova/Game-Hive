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
        Task AddGameAsync(Game game,IFormFile ImageFile, List<int> tagIds);
        Task DeleteGameAsync(int id);
        Task DeleteGameTagAsync(int gameId, int tagId);
        Task UpdateGameAsync(Game game, List<int> tagIds);
        Task<Game> GetGameWithTagsById(int id);
        Task<List<Game>> GetPendingGamesAsync();
    }
}
