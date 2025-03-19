using GameHive.Core.IServices;
using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameHive.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PublishersController : Controller
    {
        private readonly IPublisherRequestService _publisherRequestService;
        private readonly UserManager<IdentityUser> _userManager;
        public PublishersController(IPublisherRequestService publisherRequestService, UserManager<IdentityUser> userManager)
        {
            _publisherRequestService = publisherRequestService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var requests = await _publisherRequestService.GetAllAsync();
            return View(requests);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Approve(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Remove all existing roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return BadRequest("Failed to remove existing roles.");
            }

            // Assign "Company" role
            var addResult = await _userManager.AddToRoleAsync(user, "Company");
            if (!addResult.Succeeded)
            {
                return BadRequest("Failed to assign 'Company' role.");
            }
            var request = await _publisherRequestService.GetRequestByUserIdAsync(userId);
            if (request != null)
            {
                request.RequestEnums = RequestEnums.Approved;
                await _publisherRequestService.UpdateAsync(request);
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var request = await _publisherRequestService.GetRequestByUserIdAsync(userId);
            if (request != null)
            {
                await _publisherRequestService.DeleteAsync(request.RequestId);
            }

            return RedirectToAction("Index");
        }
    }
}
