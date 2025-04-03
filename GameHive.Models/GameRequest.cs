using GameHive.Models.enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Models
{
    public class GameRequest
    {
        public int Id { get; set; }
        public int? GameId { get; set; }
        public string PublisherId { get; set; }
        public RequestTypeEnums RequestType { get; set; }
        public RequestEnums Status { get; set; } = RequestEnums.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Game Game { get; set; }
        public IdentityUser Publisher { get; set; }

        public string Title { get; set; }
        public string BriefDescription { get; set; }
        public string FullDescription { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public string? GameIconUrl { get; set; }
        public string? GameHeaderUrl { get; set; }
        public ICollection<RequestTag> Tags { get; set; }
        public ICollection<RequestImage>? Images { get; set; }
    }
}
