
using GameHive.Areas.Admin.Models;
using GameHive.Models;
namespace GameHive.Areas.Admin.Models 
{ 
    public class UsersIndexViewModel
    {
        public List<UsersViewModel> Users { get; set; } = new List<UsersViewModel>();
        public IEnumerable<PublisherRequest> PublisherRequests { get; set; } = new List<PublisherRequest>();
    }
}
