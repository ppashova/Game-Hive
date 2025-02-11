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

namespace GameHive.Core.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        //private readonly string smtpServer = "smtp.gmail.com";
        //private readonly int smtpPort = 456;
        //private readonly string smtpUser = "your-email@gmail.com"; 
        //private readonly string smtpPass = "your-email-password";
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new System.Net.Mail.SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
            {
                client.Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
                client.EnableSsl = _emailSettings.EnableSSL;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SmtpUser),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
