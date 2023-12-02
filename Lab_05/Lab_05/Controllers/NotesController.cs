using Lab_05.Models;
using Lab_05.ViewModels;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;
using static System.Net.Mime.MediaTypeNames;

namespace Lab_05.Controllers
{
    [ResponseCache(CacheProfileName = "Caching")]
    public class NotesController : Controller
    {
        private readonly NotePlannerDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        private Note _currentNote;
        public NotesController(NotePlannerDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> CurrentNote(int id, int subNoteNumber = 1)
        {
            _currentNote = _db.Notes.Include("SubNotes").Include("NoteStatus").Include("NoteTag").FirstOrDefault(note => note.NoteId == id);

            if (_currentNote == null)
            {
                return NotFound();
            }

            CurrentNoteViewModel viewModel = new CurrentNoteViewModel
            {
                Note = _currentNote,
                NoteId = _currentNote.NoteId,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CurrentNote(int noteId, string subNoteDescription, bool isCompleted)
        {
            _currentNote = _db.Notes.Include("SubNotes").FirstOrDefault(note => note.NoteId == noteId);

            if (_currentNote == null)
            {
                return NotFound();
            }

            SubNote newSubNote = new SubNote
            {
                SubNoteDescription = subNoteDescription,
                NoteId = noteId,
                IsCompleted = isCompleted
            };

            _currentNote.SubNotes.Add(newSubNote);
            await _db.SaveChangesAsync();

            return RedirectToAction("CurrentNote", new { id = noteId });
        }

        [Authorize]
        public async Task<IActionResult> Index(DateTime StartDateTime, DateTime EndDateTime, DateTime CreateDateTime, string title, int NoteTag = 0, int NoteStatus = 0, int page = 1)
        {
            int pageSize = 10;
            List<Note> notes;

            if (User.IsInRole("admin"))
            {
                notes = _db.Notes.Include("NoteTag").Include("NoteStatus").Include("User").ToList();
            }
            else
            {
                ApplicationUser indexUser = await _userManager.FindByNameAsync(User.Identity.Name);
                notes = _db.Notes.Include("NoteTag").Include("NoteStatus").Include("User")
                    .Where(note => note.UserId == indexUser.Id).ToList();
            }


            if (NoteTag != 0)
            {
                notes = notes.Where(note => note.NoteTagId == NoteTag).ToList();
            }

            if (NoteStatus != 0)
            {
                notes = notes.Where(note => note.NoteStatusId == NoteStatus).ToList();
            }

            if (!string.IsNullOrEmpty(title))
            {
                notes = notes.Where(note => note.Title.Contains(title)).ToList();
            }

            if (StartDateTime != DateTime.MinValue)
            {
                notes = notes.Where(note => note.StartTime >= StartDateTime).ToList();
            }

            if (EndDateTime != DateTime.MinValue)
            {
                notes = notes.Where(note => note.EndTime <= EndDateTime).ToList();
            }

            if (CreateDateTime != DateTime.MinValue)
            {
                notes = notes.Where(note => note.CreateDate >= CreateDateTime).ToList();
            }

            notes = notes.OrderBy(note => note.EndTime).ToList();
            List<NoteStatus> noteStatuses = _db.NoteStatuses.ToList();
            List<NoteTag> noteTags = _db.NoteTags.ToList();
            noteStatuses.Insert(0, new NoteStatus { StatusName = "Всё", NoteStatusId = 0 });
            noteTags.Insert(0, new NoteTag { TagName = "Всё", NoteTagId = 0 });

            int notesCount = notes.Count;
            List<Note> listOfNotes = notes.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(notesCount, page, pageSize);
            FilterNotesViewModel filterNotesViewModel = new FilterNotesViewModel(noteStatuses, noteTags, NoteStatus, NoteTag, title, StartDateTime, EndDateTime, CreateDateTime);
            ApplicationUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            NotesViewModel notesViewModel = new NotesViewModel(listOfNotes, pageViewModel, filterNotesViewModel, currentUser, _db.SubNotes, StartDateTime, EndDateTime, CreateDateTime);

            return View(notesViewModel);
        }
        public async Task<IActionResult> SelectWeek()
        {
            DateTime now = DateTime.Now;
            int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime startOfWeek = now.AddDays(-diff).Date;
            DateTime endOfWeek = startOfWeek.AddDays(7);
            return RedirectToAction("Index", new { StartDateTime = startOfWeek, EndDateTime = endOfWeek });
        }
        public async Task<IActionResult> SelectDay()
        {
            return RedirectToAction("Index", new { StartDateTime = DateTime.Today, EndDateTime = DateTime.Today.AddDays(1).AddSeconds(-1) });
        }

        public IActionResult Create()
        {
            List<NoteStatus> noteStatuses = _db.NoteStatuses.ToList();
            List<NoteTag> noteTags = _db.NoteTags.ToList();


            CreateNoteViewModel createNoteViewModel = new CreateNoteViewModel(noteStatuses, noteTags);

            return View(createNoteViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNoteViewModel model)
        {
            ApplicationUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ModelState.IsValid)
            {
                if (model.StartDateTime > model.EndDateTime)
                {
                    ModelState.AddModelError("EndDateTime", "Дата окончания выполнения не может быть раньше даты начала выполнения.");
                    model.NoteStatuses = new SelectList(_db.NoteStatuses, "NoteStatusId", "StatusName");
                    model.NoteTags = new SelectList(_db.NoteTags, "NoteTagsId", "TagName");
                    return View(model);
                }

                Note newNote = new Note()
                {
                    Title = model.Title,
                    NoteDescription = model.Description,
                    UserId = currentUser.Id,
                    NoteStatusId = model.NoteStatusId,
                    NoteTagId = model.NoteTagId,
                    StartTime = model.StartDateTime,
                    EndTime = model.EndDateTime,
                    CreateDate = DateTime.Now,
                };

                _db.Notes.Add(newNote);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                model.NoteStatuses = new SelectList(_db.NoteStatuses, "NoteStatusId", "StatusName");
                model.NoteTags = new SelectList(_db.NoteTags, "NoteTagsId", "TagName");
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            Note editNote = await _db.Notes.FindAsync(id);

            if (editNote == null)
            {
                return NotFound();
            }

            List<NoteStatus> noteStatuses = _db.NoteStatuses.ToList();
            List<NoteTag> noteTags = _db.NoteTags.ToList();

            EditNoteViewModel model = new EditNoteViewModel(
                noteStatuses,
                noteTags,
                editNote.NoteId,
                editNote.NoteStatusId,
                editNote.NoteTagId,
                editNote.Title,
                editNote.NoteDescription,
                editNote.StartTime,
                editNote.EndTime);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditNoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                Note editNote = _db.Notes.Include("NoteTag").Include("NoteStatus").Include("User").FirstOrDefault(note => note.NoteId == model.Id);

                if (editNote != null)
                {
                    // Присвойте значения из модели в объект Note
                    editNote.Title = model.Title;
                    editNote.NoteDescription = model.Description;
                    editNote.StartTime = model.StartDateTime;
                    editNote.EndTime = model.EndDateTime;
                    editNote.NoteStatusId = model.SelectedNoteStatusId;
                    editNote.NoteTagId = model.SelectedNoteTagId;

                    // Сохранить изменения в базе данных
                    _db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            model.NoteStatuses = new SelectList(_db.NoteStatuses.ToList(), "NoteStatusId", "StatusName");
            model.NoteTags = new SelectList(_db.NoteTags.ToList(), "NoteTagId", "TagName");
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditSubNote(EditSubNoteViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                SubNote subNote = _db.SubNotes.FirstOrDefault(subNote => subNote.SubNoteId == viewModel.Id);

                if (subNote != null)
                {
                    subNote.SubNoteDescription = viewModel.SubNoteDescription;
                    subNote.IsCompleted = viewModel.IsCompleted;

                    _db.SaveChanges();

                    return RedirectToAction("CurrentNote", "Notes", new { id = subNote.NoteId });
                }
            }
            else
            {
                Console.WriteLine("--------------------------1------------" + viewModel.IsCompleted);
                List<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors).ToList();
                foreach (var error in allErrors)
                {
                    Console.WriteLine(error);
                }
            }

            return RedirectToAction("CurrentNote", "Notes", new { id = viewModel.NoteId });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Note note = _db.Notes.FirstOrDefault(note => note.NoteId == id);

            if (note != null)
            {
                _db.Notes.Remove(note);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteSubNote(int id)
        {
            SubNote subNote = _db.SubNotes.Include("Note").FirstOrDefault(subNote => subNote.SubNoteId == id);

            int noteId = subNote.Note.NoteId;

            if (subNote != null)
            {
                _db.SubNotes.Remove(subNote);
                _db.SaveChanges();
            }

            return RedirectToAction("CurrentNote", new { id = noteId });
        }
    }
}
