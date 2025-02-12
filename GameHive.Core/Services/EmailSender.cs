using GameHive.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace GameHive.Core.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;
        public EmailSender(IConfiguration configuration)
        {
            _fromEmail = configuration["EmailSettings:FromEmail"];
            _smtpClient = new SmtpClient
            {
                Host = configuration["EmailSettings:SmtpServer"],
                Port = int.Parse(configuration["EmailSettings:SmtpPort"] ?? "587"),
                Credentials = new NetworkCredential(
                    configuration["EmailSettings:SmtpUser"],
                    configuration["EmailSettings:SmtpPass"]
                ),
                EnableSsl =true
            };
        }
        public async Task SendConfirmationEmailAsync(string email, string ConfirmationLink)
        {
            string subject = "Confirm Your Email";
            string message = $"<p>Please confirm your account by <a href=\"{ConfirmationLink}\">clicking here</a>.</p>";
            var mailMessage = new MailMessage(_fromEmail, email, subject, message)
            {
                IsBodyHtml = true
            };
            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
