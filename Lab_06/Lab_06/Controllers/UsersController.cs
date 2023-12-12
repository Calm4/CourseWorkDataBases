using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab_06.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Lab_06.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private NotePlannerDbContext _db;

        public UsersController(NotePlannerDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _db.Users.ToList();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            User note = _db.Users.FirstOrDefault(user => user.UserId == id);
            if (note == null)
            {
                return NotFound();
            }

            return new ObjectResult(note);
        }
    }
}
