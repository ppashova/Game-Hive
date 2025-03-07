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
        public IFormFile? ImageFile { get; set; }
        public string BriefDescription { get; set; }
        public string FullDescription { get; set; }
    }
}