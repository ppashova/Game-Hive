using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;

namespace GameHive.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        [MaxLength(150, ErrorMessage = "You've reached the brief description limit")]
        public string? BriefDescription { get; set; }
        public string FullDescription { get; set; }
        public string GameIconUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public ICollection<UserGame> UserGames { get; set; }
        public ICollection<GameTag> GameTags { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
