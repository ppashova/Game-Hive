using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameHive.Models;

namespace GameHive.Core.IServices
{
    public interface IGameTagService
    {
        Task UpdateGameTagAsync(GameTag gt);
        Task DeleteGameTagAsync(int gameId, int tagId);
        Task AddAsync(GameTag gameTag);
    }
}
