using CloudinaryDotNet;
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
        private readonly IRepository<GameImage> _repo;

        public CloudinaryService(IOptions<CloudinarySettings> config, IRepository<GameImage> repository)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
            _repo = repository;
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

        public async Task AddMultipleImagesAsync(int id, List<string> imageURLs)
        {
            foreach(var x in imageURLs)
            {
                await _repo.AddAsync(
                    new GameImage
                    {
                        GameId = id,
                        imageURL = x
                    });
                
            }

        }
        public async Task AddSingleImageAsync(int id,string ImageURL)
        {
            await _repo.AddAsync(new GameImage { GameId = id, imageURL = ImageURL });
        }
    }
}
