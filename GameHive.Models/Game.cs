using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        public string? BriefDescription { get; set; }
        public string FullDescription { get; set; }
        public string GameIconUrl { get; set; }
        public string GameHeaderUrl { get; set; }
        public DateTime RequestTime { get; set; } = DateTime.Now;
        public double Rating { get; set; }
        public int Orders { get; set; }
        public string? PublisherId { get; set; }
        [NotMapped]
        public IFormFile? GameHeader { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public ICollection<UserGame> UserGames { get; set; }
        public ICollection<GameTag> GameTags { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<UserRating> UserRatings { get; set; }
        public IdentityUser Publisher { get; set; }
        public ICollection<GameRequest> GameRequest { get; set; }
        public ICollection<GameImage> GameImages { get; set; }
    }
}
