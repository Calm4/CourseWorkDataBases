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
    public class NoteTagsController : Controller
    {
        private NotePlannerDbContext _db;

        public NoteTagsController(NotePlannerDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IEnumerable<NoteTag> Get()
        {
            return _db.NoteTags.ToList();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            NoteTag note = _db.NoteTags.FirstOrDefault(note => note.NoteTagId == id);
            if (note == null)
            {
                return NotFound();
            }

            return new ObjectResult(note);
        }
    }
}
