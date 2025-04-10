﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using GameHive.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IGameRepository _gameRepository;
        private readonly IGameRequestRepository _gameRequestRepository;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Width(300).Height(300).Crop("fill").Gravity("face")
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult == null || uploadResult.SecureUrl == null) return null;
            return uploadResult.SecureUrl.AbsoluteUri;

        }

        public async Task<List<string>> MultipleImageUploadAsync(List<IFormFile> images)
        {
            List<string> imageUrls = new List<string>();
            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    using var stream = image.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(image.FileName, stream),
                        Transformation = new Transformation().Width(600).Height(337).Crop("fill").Gravity("face")
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        imageUrls.Add(uploadResult.SecureUrl.AbsoluteUri);
                    }
                }
            }
            return imageUrls;
        }
        public async Task<bool> AddGameImagesAsync(int gameId, List<IFormFile> images)
        {
            var imageUrls = await MultipleImageUploadAsync(images);
            if (imageUrls.Count == 0) return false;
            var gameImages = imageUrls.Select(url => new GameImage
            {
                GameId = gameId,
                imageURL = url
            });
            foreach (var image in gameImages)
                await _gameRepository.AddGameImagesAsync(image);
            return true;
        }

        public async Task<bool> AddRequestImagesAsync(int requestid, List<IFormFile> images)
        {
            var imageUrls = await MultipleImageUploadAsync(images);
            if (imageUrls.Count == 0) return false;
            var requestImages = imageUrls.Select(url => new RequestImage
            {
                RequestId = requestid,
                ImageUrl = url
            });
            foreach (var image in requestImages)
                await _gameRequestRepository.AddGameRequestImagesAsync(image);
            return true;
        }
        public async Task<string> UploadHeaderAsync(IFormFile header)
        {
            if (header == null || header.Length == 0) return null;
            using var stream = header.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(header.FileName, stream),
                Transformation = new Transformation().Width(324).Height(151).Crop("fill").Gravity("face")
            };
            var uploadresult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadresult == null || uploadresult.SecureUrl == null) return null;
            return uploadresult.SecureUrl.AbsoluteUri;
        }
    }
}
