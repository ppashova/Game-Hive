using GameHive.Core.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Metadata.Edm;
using Microsoft.AspNetCore.Builder.Extensions;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using GameHive.Models;

namespace GameHive.Core.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public EmailSender(IConfiguration configuration)
        {
            _host = configuration["EmailSettings:Host"];
            _port = int.Parse(configuration["EmailSettings:Port"] ?? "0");
            _username = configuration["EmailSettings:Username"];
            _password = configuration["EmailSettings:Password"];

            if (string.IsNullOrEmpty(_host))
                throw new ArgumentNullException(nameof(_host), "SMTP Host is not configured properly.");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var body = new TextPart(TextFormat.Html) { Text = htmlMessage };
            using var message = new MimeMessage();
            message.From.Add(new MailboxAddress(null, _username));
            message.To.Add(new MailboxAddress(null, email));
            message.Subject = subject;
            message.Body = body;

            using var client = new SmtpClient();
            await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_username, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        public async Task SendOrderConfirmationEmail(string email, Order order)
        {
            string subject = "Order Confirmation";
            string body = $@"
                <h2>Thank you for your order!</h2>
                <p>Your order ID is: <strong>{order.Id}</strong></p>
                <p>Total Price: <strong>{order.TotalPrice:C}</strong></p>
                <p>We appreciate you!</p>";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("GameHive", _username));
            message.To.Add(new MailboxAddress(null, email));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = body };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_username, _password);
                await client.SendAsync(message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

    }
}
