using Microsoft.AspNetCore.Mvc;
using GameHive.Models;
using GameHive.Models.enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Core.IServices;
using Microsoft.AspNetCore.Authorization;

namespace GameHive.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GameRequestsController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ITagService _tagService;

        public GameRequestsController(IGameService gameService, ITagService tagService)
        {
            _gameService = gameService;
            _tagService = tagService;
        }

        // GET: Admin/GameRequests
        public async Task<IActionResult> Index()
        {
            IEnumerable<Game> pendingGames = await _gameService.GetPendingGamesAsync();
            return View(pendingGames);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            game.RequestStatus = RequestEnums.Approved;

            var tags = await _tagService.GetTagsByGameIdAsync(id);
            var selectedTagsIds = tags.Select(t => t.Id).ToList();

            await _gameService.UpdateGameAsync(game, selectedTagsIds);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RejectRequest(int id)
        {
            await _gameService.DeleteGameAsync(id);

            return RedirectToAction(nameof(Index));
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
                Images = ImageUrls
            };

            return View(viewModel);
        }

    }
}
