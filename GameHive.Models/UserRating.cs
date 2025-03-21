using GameHive.Models.enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Models
{
    public class UserRating
    {
        [Key]
        public Guid RatingId { get; set; }
        public string UserId { get; set; }
        public int GameId { get; set; }
        public RatingEnums Rating { get; set; }
        public DateTime CreatedAt { get; set; }

        public IdentityUser User { get; set; }
        public Game Game { get; set; }
    }
}
