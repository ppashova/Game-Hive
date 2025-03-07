using Microsoft.AspNetCore.Mvc;
using GameHive.Models;
using GameHive.Models.enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameHive.DataAccess.Repository.IRepositories;

namespace GameHive.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GameRequestsController : Controller
    {
        private readonly IGameRepository _gameRepository; // Your repository interface

        public GameRequestsController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        // GET: Admin/GameRequests
        public async Task<IActionResult> Index()
        {
            // This method should return all games with RequestStatus == Pending.
            IEnumerable<Game> pendingGames = await _gameRepository.GetPendingGamesAsync();
            return View(pendingGames);
        }

        // POST: Admin/GameRequests/ApproveRequest
        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            Game game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            game.RequestStatus = RequestEnums.Approved;
            await _gameRepository.UpdateAsync(game);

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/GameRequests/RejectRequest
        [HttpPost]
        public async Task<IActionResult> RejectRequest(int id)
        {
            Game game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            game.RequestStatus = RequestEnums.Rejected;
            await _gameRepository.UpdateAsync(game);

            return RedirectToAction(nameof(Index));
        }
    }
}
