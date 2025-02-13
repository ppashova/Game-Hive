using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameHive.Core.IServices;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;

namespace GameHive.Core.Services
{
    public class GameTagService : IGameTagService
    {
        private readonly IGameTagRepository _gtrepo;
        public GameTagService(IGameTagRepository gtrepo)
        {
            _gtrepo = gtrepo;
        }

        public async Task UpdateGameTagAsync(GameTag gt)
        {
            await UpdateGameTagAsync(gt);
        }
        public async Task DeleteGameTagAsync(int gameId, int tagId)
        {
            await _gtrepo.DeleteAsync(gameId, tagId);
        }

        public async Task AddAsync(GameTag gameTag)
        {
            await _gtrepo.AddAsync(gameTag);
        }
    }
}
