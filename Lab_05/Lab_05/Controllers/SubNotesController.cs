using Lab_05.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_05.Controllers
{
    [Authorize(Roles = "admin")]
    [ResponseCache(CacheProfileName = "Caching")]
    public class SubNotesController : Controller
    {
        private readonly NotePlannerDbContext _db;

        public SubNotesController(NotePlannerDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            int numberRows = 5;
            List<SubNote> noteStatuses = _db.SubNotes.Include("Note").Take(numberRows).ToList();
            return PartialView(noteStatuses);
        }
    }
}
