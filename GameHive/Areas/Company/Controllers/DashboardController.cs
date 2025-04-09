using GameHive.Areas.Company.Models;
using GameHive.Core.IServices;
using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using GameHive.Core.Services;

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
        private readonly EmailSender _emailSender; // Using EmailSender directly

        public DashboardController(
            IGameService gameService,
            IGameRequestService gameRequestService,
            IOrderService orderService,
            UserManager<IdentityUser> userManager,
            IConfiguration configuration) // Inject IConfiguration for EmailSender
        {
            _gameService = gameService;
            _gameRequestService = gameRequestService;
            _orderService = orderService;
            _userManager = userManager;
            _emailSender = new EmailSender(configuration); // Initialize EmailSender
        }

        public async Task<IActionResult> Index()
        {
            var publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get publisher's games
            var publisherGames = await _gameService.GetPublisherGamesAsync(publisherId);

            // Get pending order approvals
            var pendingApprovals = await GetPendingOrderDetailsAsync(publisherId);

            // Get counts of approved and rejected orders
            var approvedCount = await GetOrderDetailsCountByStatusAsync(publisherId, PublisherApprovalStatusEnums.Approved);
            var rejectedCount = await GetOrderDetailsCountByStatusAsync(publisherId, PublisherApprovalStatusEnums.Rejected);

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
                orderId, gameId, PublisherApprovalStatusEnums.Approved, publisherId);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to approve the order.";
            }
            else
            {
                TempData["SuccessMessage"] = "Order approved successfully.";

                // Get order details to send email
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order != null)
                {
                    var customer = await _userManager.FindByIdAsync(order.UserId);
                    if (customer != null)
                    {
                        await SendOrderStatusEmail(
                            customer.Email,
                            game.Name,
                            true, // approved
                            orderId);
                    }
                }
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
                orderId, gameId, PublisherApprovalStatusEnums.Rejected, publisherId);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to reject the order.";
            }
            else
            {
                TempData["SuccessMessage"] = "Order rejected successfully.";

                // Get order details to send email
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order != null)
                {
                    var customer = await _userManager.FindByIdAsync(order.UserId);
                    if (customer != null)
                    {
                        await SendOrderStatusEmail(
                            customer.Email,
                            game.Name,
                            false, // rejected
                            orderId);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // New method to send emails for order status changes
        private async Task SendOrderStatusEmail(string email, string gameTitle, bool isApproved, Guid orderId)
        {
            string subject = isApproved
                ? $"Your order for {gameTitle} has been approved"
                : $"Your order for {gameTitle} has been rejected";

            // Common CSS styles for both email templates
            string styles = @"
                body { font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }
                .email-container { background-color: #f9f9f9; border-radius: 8px; padding: 30px; border: 1px solid #ddd; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }
                .header { text-align: center; margin-bottom: 30px; }
                .logo { max-width: 150px; margin-bottom: 15px; }
                h1 { color: #333; font-size: 24px; margin-bottom: 20px; }
                .content { background-color: #fff; padding: 25px; border-radius: 6px; margin-bottom: 25px; border: 1px solid #eee; }
                .game-title { font-weight: bold; color: #4a6ee0; }
                .order-id { background-color: #f0f0f0; padding: 5px 10px; border-radius: 15px; font-size: 14px; color: #666; }
                .message { margin: 20px 0; }
                .cta-button { display: inline-block; background-color: #4a6ee0; color: #fff; text-decoration: none; padding: 12px 25px; border-radius: 4px; font-weight: bold; margin: 15px 0; }
                .footer { text-align: center; font-size: 14px; color: #888; margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee; }
                .social-links { margin: 15px 0; }
                .social-link { text-decoration: none; margin: 0 10px; color: #4a6ee0; }
            ";

            string htmlMessage;

            if (isApproved)
            {
                htmlMessage = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Order Approved</title>
                    <style>{styles}</style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='header'>
                            <img src='https://placeholder.com/150x50' alt='GameHive Logo' class='logo'>
                            <h1>🎮 Your Game Order is Approved! 🎮</h1>
                        </div>
                        
                        <div class='content'>
                            <p>Hello Gamer,</p>
                            
                            <p class='message'>
                                Great news! Your order <span class='order-id'>#{orderId}</span> for 
                                <span class='game-title'>{gameTitle}</span> has been approved by the publisher!
                            </p>
                            
                            <p>Your digital adventure awaits - a publisher or a staff member will be in contact with you soon for further details regarding delivery.</p>
                            
                            
                            <p>If you encounter any issues, our support team is ready to help!</p>
                        </div>
                        
                        <div class='footer'>
                            <p>Thank you for choosing GameHive for your gaming needs!</p>
                            <p>&copy; 2025 GameHive. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";
            }
            else
            {
                htmlMessage = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Order Status Update</title>
                    <style>{styles}</style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='header'>
                            <img src='https://placeholder.com/150x50' alt='GameHive Logo' class='logo'>
                            <h1>Order Status Update</h1>
                        </div>
                        
                        <div class='content'>
                            <p>Hello Gamer,</p>
                            
                            <p class='message'>
                                We regret to inform you that your order <span class='order-id'>#{orderId}</span> for 
                                <span class='game-title'>{gameTitle}</span> has been rejected by the publisher.
                            </p>
                            
                            <p>This could be due to various reasons including regional restrictions, inventory issues, or publisher policies.</p>
                            
                            <p>Don't worry - you have not been charged for this order. You can check your order details and available refund information in your account dashboard.</p>
                            
                            <p style='text-align: center;'>
                                <a href='https://gamehive.com/my-orders' class='cta-button'>VIEW ORDER DETAILS</a>
                            </p>
                            
                            <p>If you have any questions, please don't hesitate to contact our customer support team who will be happy to assist you.</p>
                        </div>
                        
                        <div class='footer'>
                            <p>Thank you for your understanding and continued support of GameHive!</p>
                            <div class='social-links'>
                                <a href='#' class='social-link'>Facebook</a> | 
                                <a href='#' class='social-link'>Twitter</a> | 
                                <a href='#' class='social-link'>Instagram</a>
                            </div>
                            <p>&copy; 2025 GameHive. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";
            }

            try
            {
                await _emailSender.SendEmailAsync(email, subject, htmlMessage);
            }
            catch (Exception ex)
            {
                // Log the error but don't let it break the approval/rejection flow
                // Consider adding proper logging here
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
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
                    if (game != null && game.PublisherId == publisherId && detail.ApprovalStatus == PublisherApprovalStatusEnums.Pending)
                    {
                        result.Add(detail);
                    }
                }
            }

            return result;
        }

        private async Task<int> GetOrderDetailsCountByStatusAsync(string publisherId, PublisherApprovalStatusEnums status)
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