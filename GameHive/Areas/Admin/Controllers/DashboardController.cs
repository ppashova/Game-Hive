using GameHive.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameHive.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                GameUploadRequestsCount = 5,
                PublisherRequestsCount = 3,
                OtherRequestsCount = 8,
            };

            return View(model);
        }
    }
}
