using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GameHive.DataAccess.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _context;
        public GameRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task Add(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Game>> GetAll()
        {
            return await _context.Games.Include(g => g.GameTags).ThenInclude(gt => gt.Tag).ToListAsync();
        }


        public async Task<Game> GetById(int id)
        {
            return await _context.Games.Include(g => g.GameTags).ThenInclude(gt => gt.Tag).FirstOrDefaultAsync(g => g.GameId == id);
        }
    }
}
