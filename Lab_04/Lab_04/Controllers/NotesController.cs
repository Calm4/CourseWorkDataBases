using Lab_04.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_04.Controllers
{
    [ResponseCache(CacheProfileName = "Caching")]
    public class NotesController : Controller
    {
        private readonly NotePlannerDbContext _db;

        public NotesController(NotePlannerDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            int numberRows = 5;
            List<Note> noteStatuses = _db.Notes.Include("NoteStatus").Include("SubNotes").Include("Tag").Include("User").Take(numberRows).ToList();
            return PartialView(noteStatuses);
        }
    }
}
