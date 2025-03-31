using GameHive.Models;

namespace GameHive.Areas.Admin.Models
{
    public class GameRequestDetailsViewModel
    {
        public GameRequest GameRequest { get; set; }
        public List<Tag> Tags { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
