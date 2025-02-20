using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameHive.Models
{
    public class GameFilterViewModel
    {
        public int? Tag { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public SelectList Tags { get; set; }
        public List<Game> Games { get; set; }
    }
}
