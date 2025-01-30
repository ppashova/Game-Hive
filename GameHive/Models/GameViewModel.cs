namespace GameHive.Models
{
    public class GameViewModel
    {
        public int GameId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Tag> AvailableTags { get; set; } = new List<Tag>();
        public List<int>? SelectedTagIds { get; set; } = new List<int>();
    }
}
