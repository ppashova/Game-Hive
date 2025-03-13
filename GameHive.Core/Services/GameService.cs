using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameHive.Core.IServices;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using Microsoft.AspNetCore.Http;

namespace GameHive.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _repo;
        private readonly IGameTagRepository _gtrepo;
        private readonly CloudinaryService _cloudinaryService;
        public GameService(IGameRepository repo, IGameTagRepository gtrepo, CloudinaryService cloudinaryService)
        {
            _repo = repo;
            _gtrepo = gtrepo;
            _cloudinaryService = cloudinaryService;
        }

        public async Task AddGameAsync(Game game,IFormFile ImageFile, List<int> TagId, List<IFormFile> images)
        {
            List<string> imageUrls = new List<string>();
            if (ImageFile != null)
            {
                game.GameIconUrl = await _cloudinaryService.UploadImageAsync(ImageFile);
            }
            if(images.Count > 0)
            {
                imageUrls = await _cloudinaryService.MultipleImageUploadAsync(images);
            }

            await _repo.AddAsync(game);
            foreach (var tag in TagId)
            {
                var gameTag = new GameTag
                {
                    GameId = game.GameId,
                    TagId = tag
                };
                await _gtrepo.AddAsync(gameTag);
            }
            foreach (var image in imageUrls)
            {
                var gameImage = new GameImage
                {
                    GameId = game.GameId,
                    imageURL = image
                };
                await _repo.AddGameImagesAsync(gameImage);
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
        public async Task<List<Game>> GetPendingGamesAsync()
        {
            return await _repo.GetPendingGamesAsync();
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

        public async Task<int> GetRequestCountAsync()
        {
            return await _repo.GetRequestCountAsync();
        }
        public async Task<List<string>> GetGameImagesAsync(int id)
        {
            return await _repo.GetGameImagesAsync(id);
        }
    }
}
