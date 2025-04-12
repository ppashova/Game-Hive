using GameHive.Core.IServices;
using GameHive.Core.Services;
using GameHive.Models;
using GameHive.Models.enums;
using GameHive.Models.enums;
using GameHive.Models.Game_View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace GameHive.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IGameRequestService _gameRequestService;
        private readonly ITagService _tagService;
        private readonly CloudinaryService _cloudinaryService;
        private readonly UserManager<IdentityUser> _userManager;

        public GamesController(
            IGameService gameService,
            IGameRequestService gameRequestService,
            ITagService tagService,
            CloudinaryService cloudinaryService,
            UserManager<IdentityUser> userManager)
        {
            _gameService = gameService;
            _gameRequestService = gameRequestService;
            _tagService = tagService;
            _cloudinaryService = cloudinaryService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(GameFilterViewModel filter)
        {
            var games = await _gameService.GetAllGamesAsync();
            var query = games.AsQueryable();

            if (filter != null)
            {
                if (filter.Tag.HasValue)
                {
                    query = query.Where(game => game.GameTags.Any(gt => gt.TagId == filter.Tag.Value));
                }
                if (filter.MinPrice.HasValue && filter.MinPrice >= 0)
                {
                    query = query.Where(game => game.Price >= filter.MinPrice.Value);
                }
                if (filter.MaxPrice.HasValue && filter.MaxPrice >= filter.MinPrice)
                {
                    query = query.Where(p => p.Price <= filter.MaxPrice.Value);
                }
            }

            var model = new GameFilterViewModel
            {
                Tag = filter?.Tag,
                MinPrice = filter?.MinPrice,
                MaxPrice = filter?.MaxPrice,
                Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name"),
                Games = query.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var publisher = await _userManager.FindByIdAsync(game.PublisherId);
            var tags = await _tagService.GetTagsByGameIdAsync(id);
            var images = await _gameService.GetGameImagesAsync(id);

            var viewModel = new GameDetailsViewModel
            {
                Game = game,
                Tags = tags,
                Images = images,
                AverageRating = (double)game.Rating / 2.0,
                Publisher = publisher
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Add()
        {
            var model = new GameViewModel
            {
                AvailableTags = await _tagService.GetAllAsync()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Add(GameViewModel model)
        {
            var publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (publisherId == null)
            {
                TempData["Error"] = "You must be logged in to submit a game.";
                return RedirectToAction("Index");
            }

            var gameRequest = new GameRequest
            {
                Title = model.Name,
                Price = model.Price,
                BriefDescription = model.BriefDescription,
                FullDescription = model.FullDescription,
                PublisherId = publisherId,
                RequestType = RequestTypeEnums.Add
            };

            await _gameRequestService.AddGameRequestAsync(
                gameRequest,
                model.IconFile,
                model.SelectedTagIds,
                model.GameImages,
                model.GameHeader,
                publisherId);

            TempData["Success"] = "Your game request has been submitted for approval.";
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (game.PublisherId != publisherId)
            {
                TempData["Error"] = "You can only edit your own games.";
                return RedirectToAction("Index");
            }

            var tags = await _tagService.GetAllAsync();
            var selectedTags = await _tagService.GetTagsByGameIdAsync(id);
            var imageUrls = await _gameService.GetGameImagesAsync(id);

            var model = new GameEditViewModel
            {
                GameId = game.GameId,
                Name = game.Name,
                Price = game.Price,
                AvailableTags = tags,
                SelectedTagIds = selectedTags.Select(tag => tag.Id).ToList(),
                BriefDescription = game.BriefDescription,
                FullDescription = game.FullDescription,
                ExistingImageUrls = imageUrls.ToList(),
                ExistingIconUrl = game.GameIconUrl,
                ExistingHeaderUrl = game.GameHeaderUrl,
                ImagesToKeep = imageUrls.ToList()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Edit(GameEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableTags = await _tagService.GetAllAsync();
                if (model.GameId > 0)
                {
                    model.ExistingImageUrls = await _gameService.GetGameImagesAsync(model.GameId);
                }
                return View(model);
            }

            var game = await _gameService.GetGameByIdAsync(model.GameId);
            if (game == null)
            {
                return NotFound();
            }

            var publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (game.PublisherId != publisherId)
            {
                TempData["Error"] = "You can only update your own games.";
                return RedirectToAction("Index");
            }

            var updateRequest = new GameRequest
            {
                GameId = model.GameId,
                PublisherId = publisherId,
                RequestType = RequestTypeEnums.Edit,
                Status = RequestEnums.Pending,
                Title = model.Name,
                Price = model.Price,
                BriefDescription = model.BriefDescription,
                FullDescription = model.FullDescription,
            };

            await _gameRequestService.AddGameRequestAsync(updateRequest,model.IconFile,model.SelectedTagIds,model.GameImages ?? new List<IFormFile>(),model.GameHeader,publisherId);
            if (model.IconFile != null)
            {
                updateRequest.GameIconUrl = await _cloudinaryService.UploadImageAsync(model.IconFile);
            }

            if (model.GameHeader != null)
            {
                updateRequest.GameHeaderUrl = await _cloudinaryService.UploadHeaderAsync(model.GameHeader);
            }

            if(model.ImagesToKeep != null)
            {
                foreach (var url in model.ImagesToKeep)
                    await _gameRequestService.AddExistingImageToRequestAsync(updateRequest.Id, url);
            }
            if (model.GameImages != null && model.GameImages.Count > 0)
            {
                var newImageUrls = await _cloudinaryService.MultipleImageUploadAsync(model.GameImages);
                foreach (var url in newImageUrls)
                {
                    await _gameRequestService.AddExistingImageToRequestAsync(updateRequest.Id, url);
                }
            }

            TempData["Success"] = "Game updated successfully.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PublisherGames(string publisherId)
        {
            var publisherGames = await _gameService.GetPublisherGamesAsync(publisherId);

            if (publisherGames == null || !publisherGames.Any())
            {
                return NotFound("No games found for this publisher.");
            }

            return View(publisherGames);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateGame(int gameId, int ratingValue)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "You must be logged in to rate a game.";
                return RedirectToAction("Details", new { id = gameId });
            }

            if (ratingValue < 1 || ratingValue > 10)
            {
                TempData["Error"] = "Invalid rating. Please choose a valid rating.";
                return RedirectToAction("Details", new { id = gameId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                TempData["Error"] = "An error occurred. Please try again.";
                return RedirectToAction("Details", new { id = gameId });
            }



            var success = await _gameService.RateGameAsync(userId, gameId, ratingValue);
            if (!success)
            {
                TempData["Error"] = "Failed to save rating.";
            }
            else
            {
                TempData["Success"] = "Thank you for your rating!";
            }
            await _gameService.UpdateGameAverageRatingAsync(gameId);
            return RedirectToAction("Details", new { id = gameId });
        }
    }
}
