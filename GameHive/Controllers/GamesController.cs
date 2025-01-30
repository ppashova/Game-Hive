using GameHive.Core.IServices;
using GameHive.Core.Services;
using GameHive.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameHive.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ITagService _tagService;
        public GamesController(IGameService gameService, ITagService tagService)
        {
            _gameService = gameService;
            _tagService = tagService;
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
                Name = model.Name
            };
            await _gameService.AddGameAsync(game, model.SelectedTagIds);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetAllGamesAsync();
            return View(games);
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            //if(game == null)
            //    return NotFound();
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
                AvailableTags = tags,
                SelectedTagIds = selectedTagsId
            };


            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(GameViewModel model)
        {
            var game = await _gameService.GetGameByIdAsync(model.GameId);
            if (game == null)
                return NotFound();
            game.Name = model.Name;
            await _gameService.UpdateGameAsync(game);
            return RedirectToAction("Index");
        }

    }
}
