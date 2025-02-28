using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.Services
{
    public class SmtpOptions
    {
        public const string Smtp = "Smtp";

        public required string Host { get; set; }

        public required int Port { get; set; }
    }
}
