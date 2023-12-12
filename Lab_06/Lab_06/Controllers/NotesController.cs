using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab_06.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Lab_06.Controllers
{
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private NotePlannerDbContext _db;

        public NotesController(NotePlannerDbContext context)
        {
            _db = context;
            if (!_db.Notes.Any())
            {
                if (!_db.NoteStatuses.Any())
                {
                    _db.NoteStatuses.Add(new NoteStatus() { StatusName = "Не запланировано" });
                    _db.NoteStatuses.Add(new NoteStatus() { StatusName = "Запланировано" });
                    _db.NoteStatuses.Add(new NoteStatus() { StatusName = "Выполняется" });
                    _db.NoteStatuses.Add(new NoteStatus() { StatusName = "Выполнено" });
                    _db.SaveChanges();
                }

                if (!_db.NoteTags.Any())
                {
                    _db.NoteTags.Add(new NoteTag() { TagName = "Игры" });
                    _db.NoteTags.Add(new NoteTag() { TagName = "Фильмы" });
                    _db.NoteTags.Add(new NoteTag() { TagName = "Спорт" });
                    _db.NoteTags.Add(new NoteTag() { TagName = "Покупки" });
                    _db.SaveChanges();
                }

                if (!_db.Users.Any())
                {
                    _db.Users.Add(new User()
                    {
                        FirstName = "Adin1", LastName = "Dva1", Email = "qwe1@mail.ru", UserName = "login123",
                        UserPassword = "Pass_1"
                    });
                    _db.Users.Add(new User()
                    {
                        FirstName = "Adin2", LastName = "Dva2", Email = "qwe2@mail.ru", UserName = "login456",
                        UserPassword = "Pass_2"
                    });
                    _db.Users.Add(new User()
                    {
                        FirstName = "Adin3", LastName = "Dva3", Email = "qwe3@mail.ru", UserName = "login789",
                        UserPassword = "Pass_3"
                    });
                    _db.SaveChanges();
                }

                _db.Notes.Add(new Note()
                {
                    CreateDate = DateTime.Now, EndTime = DateTime.Now, StartTime = DateTime.Now,
                    Title = "Title_01", NoteDescription = "Description_01",
                    UserId = 1, NoteStatusId = 1, NoteTagId = 1,
                });
                
                _db.Notes.Add(new Note()
                {
                    CreateDate = DateTime.Now, EndTime = DateTime.Now, StartTime = DateTime.Now,
                    Title = "Title_02", NoteDescription = "Description_02",
                    UserId = 2, NoteStatusId = 2, NoteTagId = 2,
                });
                _db.Notes.Add(new Note()
                {
                    CreateDate = DateTime.Now, EndTime = DateTime.Now, StartTime = DateTime.Now,
                    Title = "Title_03", NoteDescription = "Description_03",
                    UserId = 1, NoteStatusId = 1, NoteTagId = 1,
                });
                _db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Note> Get()
        {
            return _db.Notes.Include("NoteStatus").Include("NoteTag").Include("User").ToList();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Note note = _db.Notes.Include("NoteStatus").Include("NoteTag").Include("User").FirstOrDefault(note => note.NoteId == id);
            if (note == null)
            {
                return NotFound();
            }

            return new ObjectResult(note);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Note note)
        {
            List<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors).ToList();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Notes.Add(note);
            _db.SaveChanges();

            note.NoteStatus = _db.NoteStatuses.FirstOrDefault((ns => ns.NoteStatusId == note.NoteStatusId));
            note.NoteTag = _db.NoteTags.FirstOrDefault((ns => ns.NoteTagId == note.NoteTagId));
            note.User = _db.Users.FirstOrDefault(u => u.UserId == note.UserId);

            return Ok(note);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Note note)
        {
            if (!_db.Notes.Any(note => note.NoteId == note.NoteId))
            {
                ModelState.AddModelError("","Заметки с таким id не существует");
            }

            if (!ModelState.IsValid)
                return BadRequest();

            _db.Update(note);
            _db.SaveChanges();

            note.NoteStatus = _db.NoteStatuses.FirstOrDefault((ns => ns.NoteStatusId == note.NoteStatusId));
            note.NoteTag = _db.NoteTags.FirstOrDefault((nt => nt.NoteTagId == note.NoteTagId));
            note.User = _db.Users.FirstOrDefault(u => u.UserId == note.UserId);
            
            return Ok(note);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Note note = _db.Notes.FirstOrDefault(note => note.NoteId == id);
            if (note == null)
            {
                return NotFound();
            }

            _db.Notes.Remove(note);
            _db.SaveChanges();
            return Ok(note);
        }
    }
}
