using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Repository.Repositories
{
    public class GameTagRepository : IGameTagRepository
    {
        private readonly ApplicationDbContext _context;
        public GameTagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(GameTag gameTag)
        {
            _context.GameTags.Add(gameTag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int gameId, int tagId)
        {
            var gameTags = _context.GameTags.Where(gt => gt.GameId == gameId && gt.TagId == tagId).ToList();
            _context.GameTags.RemoveRange(gameTags);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByTagIdAsync(int id)
        {
            var gameTag = _context.GameTags.Where(gt => gt.TagId == id).ToList();
            _context.GameTags.RemoveRange(gameTag);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteByGameIdAsync(int id)
        {
            var gameTag = _context.GameTags.Where(gt => gt.GameId == id).ToList();
            _context.GameTags.RemoveRange(gameTag);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GameTag>> GetAllAsync()
        {
            return await _context.GameTags.Include(gt => gt.Game).Include(gt => gt.Tag).ToListAsync();
        }

        public async Task<List<Tag>> GetTagsByGameIdAsync(int gameId)
        {
            return await _context.GameTags.Where(gt => gameId == gt.GameId).Select(gt => gt.Tag).ToListAsync();
        }
        public async Task UpdateGameTagAsync(GameTag tag)
        {
            _context.GameTags.Update(tag);
            await _context.SaveChangesAsync();
        }
    }
}
