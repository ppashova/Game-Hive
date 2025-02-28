namespace GameHive.Models
{
    public class GameDetailsViewModel
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
