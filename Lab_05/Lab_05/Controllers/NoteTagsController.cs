using Lab_05.Models;
using Lab_05.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab_05.Controllers
{
    [Authorize(Roles = "admin")]
    [ResponseCache(CacheProfileName = "Caching")]
    public class NoteTagsController : Controller
    {
        private readonly NotePlannerDbContext _db;

        public NoteTagsController(NotePlannerDbContext db)
        {
            _db = db;
        }


        public async Task<IActionResult> Index(string TagName)
        {
            List<NoteTag> tags = await _db.NoteTags.ToListAsync();
            var viewModel = new NoteTagsViewModel(tags, TagName);
            return View(viewModel);
        }

        public async Task<IActionResult> Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagViewModel model)
        {
            if (ModelState.IsValid)
            {
                NoteTag newNote = new NoteTag()
                {
                    TagName = model.TagName,
                };

                _db.NoteTags.Add(newNote);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Edit(int id)
        {
            NoteTag editTag = await _db.NoteTags.FindAsync(id);

            if (editTag == null)
            {
                return NotFound();
            }

            EditNoteTagViewModel model = new EditNoteTagViewModel(editTag.TagName);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditNoteTagViewModel model)
        {
            if (ModelState.IsValid)
            {
                NoteTag editTag = _db.NoteTags.FirstOrDefault(tag => tag.NoteTagId == model.Id);

                if (editTag != null)
                {
                    editTag.TagName = model.TagName;
                    _db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            NoteTag noteTag = _db.NoteTags.FirstOrDefault(tag => tag.NoteTagId == id);

            if (noteTag != null)
            {
                _db.NoteTags.Remove(noteTag);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}