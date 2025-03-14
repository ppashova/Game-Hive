﻿using GameHive.Models;
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
    }
}
