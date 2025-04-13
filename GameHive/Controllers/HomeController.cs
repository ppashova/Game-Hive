using System.Diagnostics;
using GameHive.Core.IServices;
using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace GameHive.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IGameService _gameService;
        private readonly ISupportRequestService _supportRequestService;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender, IGameService gameService, ISupportRequestService supportRequestService, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _emailSender = emailSender;
            _gameService = gameService;
            _supportRequestService = supportRequestService;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetAllGamesAsync();
            return View(games);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Publisher()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SupportTicket()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSupportTicket(SupportRequest supportRequest)
        {
            supportRequest.UserId = _userManager.GetUserId(User);
            var user = await _userManager.GetUserAsync(User);
            var userEmail = await _userManager.GetEmailAsync(user);

            await _supportRequestService.CreateRequestAsync(supportRequest);
            await SendSupportTicketEmail(userEmail, supportRequest.Subject, supportRequest.ProblemDescription, supportRequest.RequestId);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task SendSupportTicketEmail(string email, string subjectText, string description, Guid ticketId)
        {
            string subject = $"[GameHive Support] Нов тикет #{ticketId.ToString().Substring(0, 8)}";

            string styles = @" body { font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }
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
                .social-link { text-decoration: none; margin: 0 10px; color: #4a6ee0; } ";

            string htmlMessage = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Нов Support Тикет</title>
                    <style>{styles}</style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='header'>
                            <h1>🛠️ Получихме твоя support тикет!</h1>
                        </div>

                        <div class='content'>
                            <p>Здравей!</p>

                            <p class='message'>
                                Твоята заявка с номер <span class='order-id'>#{ticketId}</span> е успешно създадена.
                            </p>

                            <p><strong>Тема:</strong> {subjectText}</p>
                            <p><strong>Описание:</strong></p>
                            <p>{description}</p>

                            <p>Екипът ни ще се свърже с теб възможно най-скоро. Благодарим ти, че се обърна към нас!</p>
                        </div>

                        <div class='footer'>
                            <p>&copy; 2025 GameHive – We're here to help!</p>
                        </div>
                    </div>
                </body>
                </html>";

            try
            {
                await _emailSender.SendEmailAsync(email, subject, htmlMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send support ticket email: {ex.Message}");
            }
        }

    }
}
