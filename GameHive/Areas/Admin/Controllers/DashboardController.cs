using GameHive.Areas.Admin.Models;
using GameHive.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameHive.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IGameService _gameService;
        public DashboardController(IGameService gameService)
        {
            _gameService = gameService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                GameUploadRequestsCount = await _gameService.GetRequestCountAsync(),
                PublisherRequestsCount = 3,
                OtherRequestsCount = 8,
            };

            return View(model);
        }
    }
}
