using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace GameHive.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<UserGame> UserGames { get; set; }
        public ICollection<GameTag> GameTags { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
