using GameHive.Models;

namespace GameHive.Areas.Admin.Models
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsLocked { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> AllRoles { get; set; } = new List<string>();
        public PublisherRequest PublisherRequest { get; set; }
    }
}
