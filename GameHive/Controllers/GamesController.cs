using GameHive.Core.IServices;
using GameHive.Core.Services;
using GameHive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using System.Data.Entity;

namespace GameHive.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ITagService _tagService;
        private readonly IGameTagService _gameTagService;
        public GamesController(IGameService gameService, ITagService tagService, IGameTagService gameTagService)
        {
            _gameService = gameService;
            _tagService = tagService;
            _gameTagService = gameTagService;
        }
        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetAllGamesAsync();
            return View(games);
        }
        public async Task<IActionResult> Add()
        {
            var model = new GameViewModel
            {
                AvailableTags = await _tagService.GetAllAsync()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(GameViewModel model)
        { 
            var game = new Game
            {
                Name = model.Name,
                Price = model.Price,
                BriefDescription = model.BriefDescription,
                FullDescription = model.FullDescription
            };
            await _gameService.AddGameAsync(game, model.SelectedTagIds);
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
            var viewModel = new GameDetailsViewModel
            {
                Game = game,
                Tags = Tags
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
                FullDescription = game.FullDescription
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

    }
}
