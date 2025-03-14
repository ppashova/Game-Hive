using GameHive.Models.enums;

namespace GameHive.Models
{
    public class GameViewModel
    {
        public int GameId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<Tag> AvailableTags { get; set; } = new List<Tag>();
        public List<int>? SelectedTagIds { get; set; } = new List<int>();
        public string? GameIconUrl { get; set; }
        public string SteamLink { get; set; }
        public IFormFile? IconFile { get; set; }
        public string BriefDescription { get; set; }
        public string FullDescription { get; set; }
        public IFormFile GameHeader { get; set; }
        public List<IFormFile> GameImages { get; set; } = new List<IFormFile>();
        public RequestEnums RequestStatus { get; set; }
    }
}