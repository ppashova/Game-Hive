using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Azure;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models.enums;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GameHive.DataAccess.Repository.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _context;
        public GameRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Game>> GetPendingGamesAsync()
        {
            return await _context.Games
                .Include(g => g.GameTags)
                    .ThenInclude(gt => gt.Tag)
                .Where(g => g.RequestStatus == RequestEnums.Pending)
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games.Include(g => g.GameTags).ThenInclude(gt => gt.Tag).ToListAsync();
        }

        public async Task<Game> GetGameWithTagsById(int id)
        {
            return await _context.Games.Include(g => g.GameTags).ThenInclude(gt => gt.Tag).FirstOrDefaultAsync(g => g.GameId == id);
        }

        public async Task<Game> GetByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task UpdateAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetRequestCountAsync()
        {
            var requests = await _context.Games.Where(r => r.RequestStatus == RequestEnums.Pending).ToListAsync();
            return requests.Count;
        }
        public async Task AddGameImagesAsync(GameImage gameImage)
        {
            await _context.GameImages.AddAsync(gameImage);
            await _context.SaveChangesAsync();
        }
        public async Task<List<string>> GetGameImagesAsync(int id)
        {
            return await _context.GameImages.Where(g => id == g.GameId).Select(g => g.imageURL).ToListAsync();
        }
        public async Task<List<UserRating>> GetRatingsByGameIdAsync(int gameId)
        {
            return await _context.UserRatings
                .Where(r => r.GameId == gameId)
                .ToListAsync();
        }
        public async Task AddRatingAsync(UserRating rating)
        {
            _context.UserRatings.Add(rating);
            await _context.SaveChangesAsync();
        }
        public async Task<UserRating?> GetUserRatingAsync(string userId, int gameId)
        {
            return await _context.UserRatings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.GameId == gameId);
        }
        
        public async Task UpdateGameAverageRatingAsync(double newRating, int gameId)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game != null)
            {
                game.Rating = newRating;
                await UpdateAsync(game);
            }
        }
        public async Task DeleteRatingAsync(UserRating rating)
        {
            _context.UserRatings.Remove(rating);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserRatingAsync(UserRating rating)
        {
            _context.UserRatings.Update(rating);
            await _context.SaveChangesAsync();
        }
    }
}
