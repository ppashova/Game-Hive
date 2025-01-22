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
    internal class Game
    {
        [Key]
        int GameId { get; set; }
        [Required]
        string Name { get; set; }
        public byte[] Image { get; set; }
        public ICollection<UserGame> UserGames= new List<UserGame>();
        public ICollection<GameTag> GameTags= new List<GameTag>();



    }
}
