using CloudinaryDotNet;
using GameHive.Core.IServices;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.Services
{
    public class GameRequestService : IGameRequestService
    {
        private readonly IGameRequestRepository _requestRepo;
        private readonly IGameRepository _gameRepo;
        private readonly IGameTagRepository _gtrepo;
        private readonly CloudinaryService _cloudinaryService;

        public GameRequestService(
            IGameRequestRepository requestRepo,
            IGameRepository gameRepo,
            IGameTagRepository gtrepo,
            CloudinaryService cloudinaryService)
        {
            _requestRepo = requestRepo;
            _gameRepo = gameRepo;
            _gtrepo = gtrepo;
            _cloudinaryService = cloudinaryService;
        }

        public async Task AddGameRequestAsync(GameRequest gameRequest, IFormFile imageFile, List<int> tagIds, List<IFormFile> images, IFormFile gameHeader, string publisherId)
        {
            gameRequest.PublisherId = publisherId;

            List<string> imageUrls = new List<string>();

            if (gameHeader != null)
            {
                gameRequest.GameHeaderUrl = await _cloudinaryService.UploadHeaderAsync(gameHeader);
            }
            if (imageFile != null)
            {
                gameRequest.GameIconUrl = await _cloudinaryService.UploadImageAsync(imageFile);
            }

            if(images.Count > 0)
            {
                 imageUrls = await _cloudinaryService.MultipleImageUploadAsync(images);
            }
            // Add the request to the database
            await _requestRepo.AddRequestAsync(gameRequest);

            // Associate tags with the game request
            foreach (var tagId in tagIds)
            {
                var gameRequestTag = new RequestTag
                {
                    RequestId = gameRequest.Id,
                    TagId = tagId
                };
                await _requestRepo.AddGameRequestTagsAsync(gameRequestTag);
            }
            foreach(var url in imageUrls)
            {
                await _requestRepo.AddRequestImagesAsync(gameRequest.Id, url);
            }
        }

        public async Task<List<GameRequest>> GetPendingRequestsAsync()
        {
            return await _requestRepo.GetPendingRequestsAsync();
        }

        public async Task<bool> ApproveGameRequestAsync(int requestId)
        {
            var gameRequest = await _requestRepo.GetByIdAsync(requestId);
            if (gameRequest == null) return false;

           if(gameRequest.RequestType == RequestTypeEnums.Add)
           {
                var game = new Game
                {
                    Name = gameRequest.Title,
                    BriefDescription = gameRequest.BriefDescription,
                    FullDescription = gameRequest.FullDescription,
                    Price = gameRequest.Price,
                    GameIconUrl = gameRequest.GameIconUrl,
                    GameHeaderUrl = gameRequest.GameHeaderUrl,
                    PublisherId = gameRequest.PublisherId
                };
                await _gameRepo.AddAsync(game);
                foreach (var requestTag in gameRequest.Tags)
                {
                    await _gtrepo.AddAsync(new GameTag { GameId = game.GameId, TagId = requestTag.TagId });
                }
                foreach (var ImageUrl in gameRequest.Images)
                {
                    await _gameRepo.AddGameImageWithUrl(game.GameId, ImageUrl.ImageUrl);
                }
           }

           else if(gameRequest.RequestType == RequestTypeEnums.Edit && gameRequest.GameId.HasValue)
           {
                var game = await _gameRepo.GetByIdAsync(gameRequest.GameId.Value);
                if (game == null) return false;
                var imageUrls = await _requestRepo.GetRequestImagesAsync(requestId);

                game.Name = gameRequest.Title;
                game.BriefDescription = gameRequest.BriefDescription;
                game.FullDescription = gameRequest.FullDescription;
                game.Price = gameRequest.Price;

                if (!string.IsNullOrEmpty(gameRequest.GameIconUrl))
                {
                    game.GameIconUrl = gameRequest.GameIconUrl;
                }

                if (!string.IsNullOrEmpty(gameRequest.GameHeaderUrl))
                {
                    game.GameHeaderUrl = gameRequest.GameHeaderUrl;
                }

                await _gameRepo.UpdateAsync(game);

                await _gtrepo.DeleteByGameIdAsync(game.GameId);
                foreach (var requestTag in gameRequest.Tags)
                {
                    await _gtrepo.AddAsync(new GameTag { GameId = game.GameId, TagId = requestTag.TagId });
                }


                await _gameRepo.DeleteAllGameImagesAsync(game.GameId);
                foreach (var imageUrl in imageUrls)
                {
                    await _gameRepo.AddGameImageWithUrl(game.GameId, imageUrl);

                }
           }
            gameRequest.Status = RequestEnums.Approved;
            await _requestRepo.UpdateRequestAsync(gameRequest);

            return true;
        }


        public async Task RejectGameRequestAsync(int requestId)
        {
            var gameRequest = await _requestRepo.GetByIdAsync(requestId);
            if (gameRequest != null)
            {
                gameRequest.Status = RequestEnums.Rejected;
                await _requestRepo.UpdateRequestAsync(gameRequest);
            }
        }

        public async Task<GameRequest> GetGameRequestByIdAsync(int id)
        {
            return await _requestRepo.GetByIdAsync(id);
        }

        public async Task<List<GameRequest>> GetPublisherRequestsById(string PublisherId)
        {
            return await _requestRepo.GetPublisherRequestsById(PublisherId);
        }

        public async Task AddExistingImageToRequestAsync(int requestId, string imageUrl)
        {
            await _requestRepo.AddRequestImagesAsync(requestId, imageUrl);
        }

        public async Task<List<string>> GetRequestImagesAsync(int requestId)
        {
            return await _requestRepo.GetRequestImagesAsync(requestId);
        }
    }

}
