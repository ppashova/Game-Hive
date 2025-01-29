using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Repository
{
    public interface IGameTagRepository
    {
        Task AddAsync(GameTag gameTag);
        Task<IEnumerable<GameTag>> GetAllAsync();
        Task DeleteAsync(int gameId);
        Task DeleteByTagIdAsync(int id);
    }
}
