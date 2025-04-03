using GameHive.Areas.Admin.Models;
using GameHive.Core.IServices;
using GameHive.Core.Services;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameHive.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GameRequestsController : Controller
    {
        private readonly IGameRequestService _gameRequestService;
        private readonly ITagService _tagService;
        private readonly IGameService _gameService;

        public GameRequestsController(IGameRequestService gameRequestService, ITagService tagService, IGameService gameService)
        {
            _gameRequestService = gameRequestService;
            _tagService = tagService;
            _gameService = gameService;
        }


        // GET: Admin/GameRequests
        public async Task<IActionResult> Index()
        {
            var pendingRequests = await _gameRequestService.GetPendingRequestsAsync();
            var games = await _gameService.GetAllGamesAsync();

            var model = new GameIndexViewModel
                        {
                            GameRequests = pendingRequests,
                            Games = games
                        };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var success = await _gameRequestService.ApproveGameRequestAsync(id);
            if (!success)
            {
                TempData["Error"] = "Failed to approve the game request.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Game request approved successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RejectRequest(int id)
        {
            await _gameRequestService.RejectGameRequestAsync(id);
            TempData["Success"] = "Game request rejected.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var request = await _gameRequestService.GetGameRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            var tags = request.Tags.Select(t => t.Tag).ToList();
            var viewModel = new GameRequestDetailsViewModel
            {
                GameRequest = request,
                Tags = tags,
                ImageUrls = request.Images.Select(i => i.ImageUrl).ToList()
            };

            return View(viewModel);
        }

        
        public async Task<IActionResult> Delete(int gameId)
        {
            var game = await _gameService.GetGameByIdAsync(gameId);
            await _gameService.DeleteGameAsync(gameId, game.PublisherId);
            return RedirectToAction("Index");
        }

    }
}
