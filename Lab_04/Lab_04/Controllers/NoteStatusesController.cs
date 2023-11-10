using Lab_04.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_04.Controllers
{
    [ResponseCache(CacheProfileName = "Caching")]
    public class NoteStatusesController : Controller
    {
        private readonly NotePlannerDbContext _db;

        public NoteStatusesController(NotePlannerDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            int numberRows = 5;
            List<NoteStatus> noteStatuses = _db.NoteStatuses.Include("Notes").Take(numberRows).ToList();
            return PartialView(noteStatuses);
        }
    }
}
