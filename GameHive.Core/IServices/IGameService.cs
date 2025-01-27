using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GameHive.Models;

namespace GameHive.Core.IServices
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllGames();
        Task<Game> GetGameById(int id);
        Task AddGame(Game game);
        Task DeleteGame(int id);
        Task UpdateGame(Game game);
    }
}
