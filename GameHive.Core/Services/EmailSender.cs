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

namespace GameHive.Core.Services
{
    public class EmailSender(IOptions<SmtpOptions> options) : IEmailSender
    {
        private readonly SmtpOptions _options = options.Value;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var body = new TextPart(TextFormat.Html);
            body.Text = htmlMessage;

            using var message = new MimeMessage();
            message.From.Add(new MailboxAddress(null, "gamehive@yahoo.com"));
            message.To.Add(new MailboxAddress(null, email));
            message.Subject = subject;
            message.Body = body;

            using var client = new SmtpClient();
            await client.ConnectAsync(_options.Host, _options.Port);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
