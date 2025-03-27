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

        public async Task<IActionResult> Index(GameFilterViewModel filter)
        {
            var games = await _gameService.GetAllGamesAsync();
            var query = games.AsQueryable();

            if (filter != null)
            {
                if (filter.Tag.HasValue)
                {
                    query = query.Where(game => game.GameTags.Any(gt => gt.TagId == filter.Tag.Value));
                }
                if (filter.MinPrice.HasValue && filter.MinPrice >= 0)
                {
                    query = query.Where(game => game.Price >= filter.MinPrice.Value);
                }
                if (filter.MaxPrice.HasValue && filter.MaxPrice >= filter.MinPrice)
                {
                    query = query.Where(p => p.Price <= filter.MaxPrice.Value);
                }
            }

            var model = new GameFilterViewModel
            {
                Tag = filter?.Tag,
                MinPrice = filter?.MinPrice,
                MaxPrice = filter?.MaxPrice,
                Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name"),
                Games = query.ToList()
            };
            return View(model);
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Add()
        {
            var model = new GameViewModel { AvailableTags = await _tagService.GetAllAsync() };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Add(GameViewModel model, List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.IconFile == null)
            {
                ModelState.AddModelError("IconFile", "Icon file is required.");
                return View(model);
            }
            if (model.SelectedTagIds == null || !model.SelectedTagIds.Any())
            {
                ModelState.AddModelError("SelectedTagIds", "At least one tag must be selected.");
                return View(model);
            }

            var game = new Game
            {
                Name = model.Name,
                Price = model.Price >= 0 ? model.Price : throw new ArgumentException("Price must be non-negative"),
                BriefDescription = model.BriefDescription,
                FullDescription = model.FullDescription,
                SteamLink = model.SteamLink
            };
            await _gameService.AddGameAsync(game, model.IconFile, model.SelectedTagIds, model.GameImages, model.GameHeader);
            return RedirectToAction("Index");
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

            var model = new GameViewModel
            {
                GameId = game.GameId,
                Name = game.Name,
                Price = game.Price,
                AvailableTags = tags,
                SelectedTagIds = selectedTags.Select(tag => tag.Id).ToList(),
                BriefDescription = game.BriefDescription,
                FullDescription = game.FullDescription,
                RequestStatus = game.RequestStatus
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var game = await _gameService.GetGameByIdAsync(model.GameId);
            if (game == null)
            {
                return NotFound();
            }

            game.Name = model.Name;
            game.Price = model.Price >= 0 ? model.Price : throw new ArgumentException("Price must be non-negative");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateGame(int gameId, int ratingValue)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "You must be logged in to rate a game.";
                return RedirectToAction("Details", new { id = gameId });
            }

            if (ratingValue < 1 || ratingValue > 10)
            {
                TempData["Error"] = "Invalid rating. Please choose a valid rating.";
                return RedirectToAction("Details", new { id = gameId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                TempData["Error"] = "An error occurred. Please try again.";
                return RedirectToAction("Details", new { id = gameId });
            }



            var success = await _gameService.RateGameAsync(userId, gameId, ratingValue);
            if (!success)
            {
                TempData["Error"] = "Failed to save rating.";
            }
            else
            {
                TempData["Success"] = "Thank you for your rating!";
            }
            await _gameService.UpdateGameAverageRatingAsync(gameId);
            return RedirectToAction("Details", new { id = gameId });
        }
    }
}
