using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GameHive.Models
{
    public class UserGame
    {
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
