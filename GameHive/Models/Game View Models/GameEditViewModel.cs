using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameHive.Models.Game_View_Models
{
    public class GameEditViewModel
    {
        public int GameId { get; set; }

        [Required]
        [Display(Name = "Game Name")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Brief Description")]
        [StringLength(300, MinimumLength = 10)]
        public string BriefDescription { get; set; }

        [Required]
        [Display(Name = "Full Description")]
        [StringLength(6000, MinimumLength = 50)]
        public string FullDescription { get; set; }

        [Display(Name = "Game Icon")]
        public IFormFile IconFile { get; set; }

        [Display(Name = "Game Header")]
        public IFormFile GameHeader { get; set; }

        [Display(Name = "Game Images")]
        public List<IFormFile> GameImages { get; set; } = new List<IFormFile>();

        [Display(Name = "Tags")]
        public List<int> SelectedTagIds { get; set; } = new List<int>();

        public IEnumerable<Tag> AvailableTags { get; set; } = new List<Tag>();

        // Properties for existing content - using strings for image URLs
        public List<string> ExistingImageUrls { get; set; } = new List<string>();
        public string ExistingIconUrl { get; set; }
        public string ExistingHeaderUrl { get; set; }

        // URLs of images to keep during edit
        public List<string> ImagesToKeep { get; set; } = new List<string>();
    }
}
