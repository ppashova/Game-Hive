﻿using GameHive.Areas.Company.Models;
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
                    <title>Поръчката е одобрена</title>
                    <style>{styles}</style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='header'>
                            <img src='https://placeholder.com/150x50' alt='Лого на GameHive' class='logo'>
                            <h1>🎮 Поръчката ти за игра е одобрена! 🎮</h1>
                        </div>
        
                        <div class='content'>
                            <p>Здравей!</p>
            
                            <p class='message'>
                                Страхотна новина! Поръчката ти с номер <span class='order-id'>#{orderId}</span> за 
                                <span class='game-title'>{gameTitle}</span> беше одобрена от издателя!
                            </p>
            
                            <p>Твоето дигитално приключение те очаква – издател или член на екипа ни скоро ще се свърже с теб за допълнителни детайли относно доставката.</p>
            
                            <p>Ако срещнеш някакви проблеми, нашият екип за поддръжка е винаги насреща!</p>
                        </div>
        
                        <div class='footer'>
                            <p>Благодарим ти, че избра GameHive за твоите гейминг нужди!</p>
                            <p>&copy; 2025 GameHive. Всички права запазени.</p>
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
                        <title>Актуализация на статуса на поръчката</title>
                        <style>{styles}</style>
                    </head>
                    <body>
                        <div class='email-container'>
                            <div class='header'>
                                <img src='https://placeholder.com/150x50' alt='Лого на GameHive' class='logo'>
                                <h1>Актуализация на статуса на поръчката</h1>
                            </div>
        
                            <div class='content'>
                                <p>Здравей, геймър!</p>
            
                                <p class='message'>
                                    За съжаление трябва да те уведомим, че поръчката ти с номер <span class='order-id'>#{orderId}</span> за 
                                    <span class='game-title'>{gameTitle}</span> беше отхвърлена от издателя.
                                </p>
            
                                <p>Това може да се дължи на различни причини, включително регионални ограничения, липса на наличност или политики на издателя.</p>
            
                                <p>Не се притеснявай – не си таксуван за тази поръчка. Можеш да провериш детайлите на поръчката и информация за възстановяване на средства в таблото си за управление.</p>
            
                                <p style='text-align: center;'>
                                    <a href='https://gamehive.com/my-orders' class='cta-button'>ВИЖ ДЕТАЙЛИТЕ НА ПОРЪЧКАТА</a>
                                </p>
            
                                <p>Ако имаш въпроси, не се колебай да се свържеш с нашия екип по поддръжка – с радост ще ти помогнем.</p>
                            </div>
        
                            <div class='footer'>
                                <p>Благодарим ти за разбирането и за това, че продължаваш да подкрепяш GameHive!</p>
                                <p>&copy; 2025 GameHive. Всички права запазени.</p>
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