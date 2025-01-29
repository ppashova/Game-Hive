using GameHive.Core.IServices;
using GameHive.Core.Services;
using GameHive.Models;
using Microsoft.AspNetCore.Mvc;
//using GameHive.Core.IServices;
//using GameHive.Core.Services;

namespace GameHive.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }
        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetAllAsync();
            return View(tags);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Tag tag)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(tag);
            //}

            await _tagService.AddAsync(tag);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null) return NotFound();
            return View(tag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tag tag)
        {
            if (id != tag.Id) return BadRequest();
            if(ModelState.IsValid)
            {
                await _tagService.UpdateAsync(tag);
                return RedirectToAction("Index");
            }
            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
