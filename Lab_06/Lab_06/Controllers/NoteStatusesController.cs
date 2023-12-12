using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lab_06.Models;

namespace Lab_06.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteStatusesController : Controller
    {
        private NotePlannerDbContext _db;

        public NoteStatusesController(NotePlannerDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IEnumerable<NoteStatus> Get()
        {
            return _db.NoteStatuses.ToList();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            NoteStatus note = _db.NoteStatuses.FirstOrDefault(note => note.NoteStatusId == id);
            if (note == null)
            {
                return NotFound();
            }

            return new ObjectResult(note);
        }
    }
}
