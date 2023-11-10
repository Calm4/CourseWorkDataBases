using Lab_04.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_04.Controllers
{
    [ResponseCache(CacheProfileName = "Caching")]
    public class SubNotesController : Controller
    {
        private readonly NotePlannerDbContext _db;

        public SubNotesController(NotePlannerDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            int numberRows = 5;
            List<SubNote> noteStatuses = _db.SubNotes.Include("Note").Take(numberRows).ToList();
            return PartialView(noteStatuses);
        }
    }
}
