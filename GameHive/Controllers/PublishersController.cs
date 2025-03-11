using GameHive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using GameHive.DataAccess;
using GameHive.Core.IServices;

public class PublishersController : Controller
{
    private readonly IPublisherRequestService _publisherRequestService;
    private readonly UserManager<IdentityUser> _userManager;

    public PublishersController(IPublisherRequestService publisherRequestService, UserManager<IdentityUser> userManager)
    {
        _publisherRequestService = publisherRequestService;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new PublisherRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Index(PublisherRequest request)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        request.UserId = user.Id;
        request.DateCreated = DateTime.Now;

        await _publisherRequestService.AddAsync(request);

        return RedirectToAction("Success");
    }

    public IActionResult Success()
    {
        return View();
    }
}
