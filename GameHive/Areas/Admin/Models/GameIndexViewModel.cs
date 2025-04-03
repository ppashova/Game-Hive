using GameHive.Models;

namespace GameHive.Areas.Admin.Models
{
    public class GameIndexViewModel
    {
        public List<GameRequest>? GameRequests { get; set; }
        public IEnumerable<Game>? Games { get; set; }
    }
}
