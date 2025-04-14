using GameHive.Areas.Admin.Models;
using GameHive.Core.IServices;
using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHive.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PublishersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPublisherRequestService _publisherRequestService;

        public PublishersController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IPublisherRequestService publisherRequestService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _publisherRequestService = publisherRequestService;
        }

        public async Task<IActionResult> Index()
        {

            var users = await _userManager.Users.ToListAsync();
            var model = new List<UsersViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                model.Add(new UsersViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = roles.ToList()
                });
            }


            var publisherRequests = await _publisherRequestService.GetAllAsync();

            var viewModel = new UsersIndexViewModel
            {
                Users = model,
                PublisherRequests = publisherRequests
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new UserDetailsViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList(),
                AllRoles = allRoles
            };


            var publisherRequest = await _publisherRequestService.GetRequestByUserIdAsync(user.Id);
            if (publisherRequest != null)
            {
                model.PublisherRequest = publisherRequest;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRoles(string userId, List<string> roles)
        {
            if (roles == null)
            {
                roles = new List<string>();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);


            foreach (var role in userRoles)
            {
                if (!roles.Contains(role))
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
            }


            foreach (var role in roles)
            {
                if (!userRoles.Contains(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            return RedirectToAction(nameof(Details), new { id = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmail(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Details), new { id = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddYears(100));

            return RedirectToAction(nameof(Details), new { id = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.SetLockoutEndDateAsync(user, null);

            return RedirectToAction(nameof(Details), new { id = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovePublisher(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }


            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }


            await _userManager.AddToRoleAsync(user, "Company");

            var request = await _publisherRequestService.GetRequestByUserIdAsync(userId);
            if (request != null)
            {
                request.RequestEnums = RequestEnums.Approved;
                await _publisherRequestService.UpdateAsync(request);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectPublisher(string userId)
        {
            var request = await _publisherRequestService.GetRequestByUserIdAsync(userId);
            if (request != null)
            {
                request.RequestEnums = RequestEnums.Rejected;
                await _publisherRequestService.UpdateAsync(request);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePublisherRequest(string userId)
        {
            var request = await _publisherRequestService.GetRequestByUserIdAsync(userId);
            if (request != null)
            {
                await _publisherRequestService.DeleteAsync(request.RequestId);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}