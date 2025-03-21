using CloudinaryDotNet.Actions;
using GameHive.Core.IServices;
using GameHive.Core.Services;
using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;

namespace GameHive.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ITagService _tagService;
        private readonly IGameTagService _gameTagService;
        private readonly CloudinaryService _cloudinaryService;
        public GamesController(IGameService gameService, ITagService tagService, IGameTagService gameTagService, CloudinaryService cloudinaryService)
        {
            _gameService = gameService;
            _tagService = tagService;
            _gameTagService = gameTagService;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetAllGamesAsync();
            return View(games);
        }
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Add()
        {
            var model = new GameViewModel
            {
                AvailableTags = await _tagService.GetAllAsync()
            };
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Add(GameViewModel model, List<IFormFile> images)
        {
            if (model.IconFile == null)
            {
                return BadRequest("Icon file is required.");
            }
            if (model.SelectedTagIds == null || !model.SelectedTagIds.Any())
            {
                return BadRequest("At least one tag must be selected.");
            }

            var game = new Game
            {
                Name = model.Name,
                Price = model.Price,
                BriefDescription = model.BriefDescription,
                FullDescription = model.FullDescription,
                SteamLink = model.SteamLink
            };
            await _gameService.AddGameAsync(game, model.IconFile, model.SelectedTagIds, model.GameImages, model.GameHeader);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Filter(GameFilterViewModel? filter)
        {
            var games = await _gameService.GetAllGamesAsync();
            var query = games.AsQueryable();

            if (filter.Tag != null)
            {
                query = query.Where(game => game.GameTags.Any(gt => gt.TagId == filter.Tag.Value));
            }
            if (filter.MinPrice != null)
            {
                query = query.Where(game => game.Price >= filter.MinPrice.Value);
            }
            if(filter.MaxPrice != null)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            var model = new GameFilterViewModel
            {
                Tag = filter.Tag,
                MinPrice = filter.MinPrice,
                MaxPrice = filter.MaxPrice,
                Tags = new SelectList(_tagService.GetAllAsync().Result, "Id", "Name"),
                Games = query.Include(game => game.GameTags).ToList()
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            var Tags = await _tagService.GetTagsByGameIdAsync(id);
            var ImageUrls = await _gameService.GetGameImagesAsync(id);
            var viewModel = new GameDetailsViewModel
            {
                Game = game,
                Tags = Tags,
                Images = ImageUrls,
                AverageRating = (double)game.Rating / 2.0
            };

            return View(viewModel);
        }


        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            var tags = await _tagService.GetAllAsync();
            var selectedTags = await _tagService.GetTagsByGameIdAsync(id);
            var selectedTagsId = selectedTags.Select(tag => tag.Id).ToList();
            var model = new GameViewModel
            {
                GameId = game.GameId,
                Name = game.Name,
                Price = game.Price,
                AvailableTags = tags,
                SelectedTagIds = selectedTagsId,
                BriefDescription = game.BriefDescription,
                FullDescription = game.FullDescription,
                RequestStatus = game.RequestStatus
            };


            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(GameViewModel model)
        {
            var game = await _gameService.GetGameByIdAsync(model.GameId);
            if (game == null)
            {
                return NotFound();
            }
            game.Name = model.Name;
            game.Price = model.Price;
            game.BriefDescription = model.BriefDescription;
            game.FullDescription = model.FullDescription;
            await _gameService.UpdateGameAsync(game, model.SelectedTagIds);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _gameService.DeleteGameAsync(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateGame(int gameId, int ratingValue)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "Трябва да влезете в профила си, за да оцените игра.";
                return RedirectToAction("Details", new { id = gameId });
            }

            if (ratingValue != 1 && ratingValue != 2 && ratingValue != 3 && ratingValue != 4 && ratingValue != 5 && ratingValue != 6 && ratingValue != 7 && ratingValue != 8 && ratingValue != 9 && ratingValue != 10)
            {
                TempData["Error"] = "Невалидна оценка. Моля, изберете валидна оценка.";
                return RedirectToAction("Details", new { id = gameId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                TempData["Error"] = "Възникна грешка. Моля, опитайте отново.";
                return RedirectToAction("Details", new { id = gameId });
            }

            var success = await _gameService.RateGameAsync(userId, gameId, ratingValue);

            if (!success)
            {
                TempData["Error"] = "Неуспешно записване на оценка.";
            }
            else
            {
                TempData["Success"] = "Благодарим за вашата оценка!";
            }
            await _gameService.UpdateGameAverageRatingAsync(gameId);
            return RedirectToAction("Details", new { id = gameId });
        }
    }
}
