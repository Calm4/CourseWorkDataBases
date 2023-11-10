using Lab_04.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_04.Controllers
{
    [ResponseCache(CacheProfileName = "Caching")]
    public class NoteTagsController : Controller
    {
        private readonly NotePlannerDbContext _db;

        public NoteTagsController(NotePlannerDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            int numberRows = 5;
            List<NoteTag> noteStatuses = _db.NoteTags.Include("Notes").Take(numberRows).ToList();
            return PartialView(noteStatuses);
        }
    }
}
