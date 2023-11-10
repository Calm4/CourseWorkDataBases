using Lab_04.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_04.Controllers
{
    public class UsersController : Controller
    {
        private readonly NotePlannerDbContext _db;

        public UsersController(NotePlannerDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            int numberRows = 5;
            List<User> noteStatuses = _db.Users.Include("Notes").Take(numberRows).ToList();
            return PartialView(noteStatuses);
        }
    }
}
