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
        public GameService(IGameRepository repo, IGameTagRepository gtrepo)
        {
            _repo = repo;
            _gtrepo = gtrepo;
        }

        public async Task AddGameAsync(Game game, List<int> TagId)
        {
            await _repo.AddAsync(game);
            foreach(var tag in TagId)
            {
                var gameTag = new GameTag
                {
                    GameId = game.GameId,
                    TagId = tag
                };
                await _gtrepo.AddAsync(gameTag);
            }
        }

        public async Task DeleteGameAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task DeleteGameTagAsync(int gameId,int tagId)
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

        public async Task UpdateGameAsync(Game game, List<int> Tags)
        {
            await _repo.UpdateAsync(game);
            await _gtrepo.DeleteByGameIdAsync(game.GameId);
            foreach (var tag in Tags)
            {
                var gameTag = new GameTag
                {
                    GameId = game.GameId,
                    TagId = tag
                };
                await _gtrepo.AddAsync(gameTag);
            }
        }
    }
}
