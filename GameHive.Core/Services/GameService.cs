using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameHive.Core.IServices;
using GameHive.DataAccess.Repository;
using GameHive.Models;

namespace GameHive.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _repo;
        private readonly GameHive.DataAccess.Repository.IGameTagRepository _gtrepo;
        public async Task AddGame(Game game, int TagId)
        {
            var GameTag = new GameTag()
            {
                GameId = game.GameId,
                TagId = TagId
            };
            await _gtrepo.AddAsync(GameTag);
        }

        public async Task DeleteGame(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<IEnumerable<Game>> GetAllGames()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Game> GetGameById(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task UpdateGame(Game game)
        {
            await _repo.UpdateAsync(game);
        }
    }
}
