using GameHive.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameHive.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PublishersController : Controller
    {
        private readonly IPublisherRequestService _publisherRequestService;
        public PublishersController(IPublisherRequestService publisherRequestService)
        {
            _publisherRequestService = publisherRequestService;
        }
        public async Task<IActionResult> Index()
        {
            var requests = await _publisherRequestService.GetAllAsync();
            return View(requests);
        }
    }
}
