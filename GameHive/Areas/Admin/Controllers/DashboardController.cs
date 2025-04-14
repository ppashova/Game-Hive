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
        private readonly IGameRequestService _gameRequestService;
        private readonly IPublisherRequestService _publisherRequestService;
        private readonly ISupportRequestService _SupportRequestService;

        public DashboardController(
            IGameService gameService,
            IOrderService orderService,
            UserManager<IdentityUser> userManager,
            IGameRequestService gameRequestService,
            IPublisherRequestService publisherRequestService,
            ISupportRequestService supportRequestService)
        {
            _gameService = gameService;
            _orderService = orderService;
            _userManager = userManager;
            _gameRequestService = gameRequestService;
            _publisherRequestService = publisherRequestService;
            _SupportRequestService = supportRequestService;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetAllGamesAsync();
            var pendingGames = await _gameRequestService.GetPendingRequestsAsync();

            var companyUsers = await _userManager.GetUsersInRoleAsync("Company");
            var pendingPublishers = await _publisherRequestService.GetAllAsync();

            var orders = await _orderService.GetAllOrdersAsync();
            var otherRequests = orders.Count(o => o.Status == OrderStatusEnums.Pending);

            var model = new DashboardViewModel
            {
                GameUploadRequestsCount = pendingGames.Count,
                PublisherRequestsCount = pendingPublishers.Count,
                OtherRequestsCount = otherRequests,
                TotalRequests = pendingGames.Count + pendingPublishers.Count + otherRequests,
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
        public async Task<IActionResult> SupportTickets()
        {
            List<SupportRequestViewModel> supportRequestViewModels = new List<SupportRequestViewModel>();
            var supportRequests = await _SupportRequestService.GetAllRequestsAsync();
            foreach (var request in supportRequests)
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                supportRequestViewModels.Add(new SupportRequestViewModel
                {
                    RequestId = request.RequestId.ToString(),
                    UserEmail = user?.Email,
                    Subject = request.Subject,
                    ProblemDescription = request.ProblemDescription
                });
            }

            return View(supportRequestViewModels);
        }
    }
}