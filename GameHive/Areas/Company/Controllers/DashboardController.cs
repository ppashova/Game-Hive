using GameHive.Areas.Company.Models;
using GameHive.Core.IServices;
using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameHive.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = "Company")]
    public class DashboardController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IGameRequestService _gameRequestService;
        private readonly IOrderService _orderService;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(
            IGameService gameService,
            IGameRequestService gameRequestService,
            IOrderService orderService,
            UserManager<IdentityUser> userManager)
        {
            _gameService = gameService;
            _gameRequestService = gameRequestService;
            _orderService = orderService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get publisher's games
            var publisherGames = await _gameService.GetPublisherGamesAsync(publisherId);

            // Get pending order approvals
            var pendingApprovals = await GetPendingOrderDetailsAsync(publisherId);

            // Get counts of approved and rejected orders
            var approvedCount = await GetOrderDetailsCountByStatusAsync(publisherId, PublisherApprovalStatus.Approved);
            var rejectedCount = await GetOrderDetailsCountByStatusAsync(publisherId, PublisherApprovalStatus.Rejected);

            // Create dashboard view model
            var dashboardVM = new PublisherDashboardViewModel
            {
                PublishedGames = publisherGames,
                TotalGames = publisherGames.Count,
                TotalSales = await CalculateTotalSalesAsync(publisherId),
                GameRequests = await _gameRequestService.GetPublisherRequestsById(publisherId),
                TopGames = GetTopRatedGames(publisherGames),
                PendingApprovals = pendingApprovals,
                TotalApprovedOrders = approvedCount,
                TotalRejectedOrders = rejectedCount
            };

            return View(dashboardVM);
        }

        [HttpGet]
        public async Task<IActionResult> GameRequests()
        {
            var publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var requests = await _gameRequestService.GetPublisherRequestsById(publisherId);
            return View(requests);
        }

        [HttpGet]
        public async Task<IActionResult> GameAnalytics(int gameId)
        {
            var publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verify game belongs to publisher
            var game = await _gameService.GetGameByIdAsync(gameId);
            if (game == null || game.PublisherId != publisherId)
            {
                return Forbid();
            }

            var analyticsVM = new GameAnalyticsViewModel
            {
                Game = game,
                TotalSales = await CalculateGameSalesAsync(gameId),
            };

            return View(analyticsVM);
        }

        // Order approval actions
        [HttpPost]
        public async Task<IActionResult> ApproveOrder(Guid orderId, int gameId)
        {
            var publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verify game belongs to publisher
            var game = await _gameService.GetGameByIdAsync(gameId);
            if (game == null || game.PublisherId != publisherId)
            {
                return Forbid();
            }

            var result = await _orderService.UpdateOrderDetailApprovalStatusAsync(
                orderId, gameId, PublisherApprovalStatus.Approved, publisherId);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to approve the order.";
            }
            else
            {
                TempData["SuccessMessage"] = "Order approved successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RejectOrder(Guid orderId, int gameId)
        {
            var publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verify game belongs to publisher
            var game = await _gameService.GetGameByIdAsync(gameId);
            if (game == null || game.PublisherId != publisherId)
            {
                return Forbid();
            }

            var result = await _orderService.UpdateOrderDetailApprovalStatusAsync(
                orderId, gameId, PublisherApprovalStatus.Rejected, publisherId);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to reject the order.";
            }
            else
            {
                TempData["SuccessMessage"] = "Order rejected successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<decimal> CalculateTotalSalesAsync(string publisherId)
        {
            // Get publisher's games
            var publisherGames = await _gameService.GetPublisherGamesAsync(publisherId);
            var gameIds = publisherGames.Select(g => g.GameId).ToList();

            // Calculate total sales from orders
            decimal totalSales = 0;
            var allOrders = await _orderService.GetAllOrdersAsync();

            foreach (var order in allOrders)
            {
                var orderDetails = order.OrderDetails.Where(od => gameIds.Contains(od.GameId));
                foreach (var detail in orderDetails)
                {
                    var game = publisherGames.FirstOrDefault(g => g.GameId == detail.GameId);
                    if (game != null)
                    {
                        totalSales += game.Price * detail.Quantity;
                    }
                }
            }

            return totalSales;
        }

        private async Task<decimal> CalculateGameSalesAsync(int gameId)
        {
            decimal totalSales = 0;
            var game = await _gameService.GetGameByIdAsync(gameId);

            if (game == null)
            {
                return 0;
            }

            var allOrders = await _orderService.GetAllOrdersAsync();

            foreach (var order in allOrders)
            {
                var orderDetails = order.OrderDetails.Where(od => od.GameId == gameId);
                foreach (var detail in orderDetails)
                {
                    totalSales += game.Price * detail.Quantity;
                }
            }

            return totalSales;
        }

        private List<Game> GetTopRatedGames(List<Game> games)
        {
            return games.OrderByDescending(g => g.Rating).Take(5).ToList();
        }

        // Helper methods for order approvals
        private async Task<List<OrderDetail>> GetPendingOrderDetailsAsync(string publisherId)
        {
            var result = new List<OrderDetail>();
            var allOrders = await _orderService.GetAllOrdersAsync();

            foreach (var order in allOrders)
            {
                foreach (var detail in order.OrderDetails)
                {
                    var game = await _gameService.GetGameByIdAsync(detail.GameId);
                    if (game != null && game.PublisherId == publisherId && detail.ApprovalStatus == PublisherApprovalStatus.Pending)
                    {
                        result.Add(detail);
                    }
                }
            }

            return result;
        }

        private async Task<int> GetOrderDetailsCountByStatusAsync(string publisherId, PublisherApprovalStatus status)
        {
            int count = 0;
            var allOrders = await _orderService.GetAllOrdersAsync();

            foreach (var order in allOrders)
            {
                foreach (var detail in order.OrderDetails)
                {
                    var game = await _gameService.GetGameByIdAsync(detail.GameId);
                    if (game != null && game.PublisherId == publisherId && detail.ApprovalStatus == status)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}