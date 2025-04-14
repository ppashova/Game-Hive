using GameHive.Areas.Admin.Models;
using GameHive.Core.IServices;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GameHive.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IOrderService _orderService;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(
            IGameService gameService,
            IOrderService orderService,
            UserManager<IdentityUser> userManager)
        {
            _gameService = gameService;
            _orderService = orderService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetAllGamesAsync();
            var pendingGames = games.Count(g => g.GameRequest != null && g.GameRequest.Any(gr => gr.Status == RequestEnums.Pending));

            var companyUsers = await _userManager.GetUsersInRoleAsync("Company");
            var pendingPublishers = companyUsers.Where(u => !u.EmailConfirmed).Count();

            var orders = await _orderService.GetAllOrdersAsync();
            var otherRequests = orders.Count(o => o.Status == OrderStatusEnums.Pending);

            var model = new DashboardViewModel
            {
                GameUploadRequestsCount = pendingGames,
                PublisherRequestsCount = pendingPublishers,
                OtherRequestsCount = otherRequests,
                TotalRequests = pendingGames + pendingPublishers + otherRequests,
                RecentOrders = orders
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .Select(o => new OrderSummary
                    {
                        Id = o.Id,
                        Amount = o.TotalPrice,
                        Date = o.OrderDate,
                        UserEmail = o.Email,
                        UserName = $"{o.FirstName} {o.LastName}"
                    }).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            var games = await _gameService.GetAllGamesAsync();
            var orders = await _orderService.GetAllOrdersAsync();

            var monthlyUploads = games
                .GroupBy(g => new { Month = g.RequestTime.Month, Year = g.RequestTime.Year })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new { Date = $"{g.Key.Month}/{g.Key.Year}", Count = g.Count() })
                .Take(7)
                .ToDictionary(x => x.Date, x => x.Count);

            var monthlySales = orders
                .GroupBy(o => new { Month = o.OrderDate.Month, Year = o.OrderDate.Year })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new { Date = $"{g.Key.Month}/{g.Key.Year}", Amount = g.Sum(o => (int)o.TotalPrice) })
                .Take(7)
                .ToDictionary(x => x.Date, x => x.Amount);

            return Json(new
            {
                uploads = monthlyUploads,
                sales = monthlySales
            });
        }
    }
}