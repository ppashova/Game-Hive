using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.IServices
{
    public interface IEmailSender
    {
        Task SendConfirmationEmailAsync(string email, string ConfirmationLink);
    }
}
