using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WebApplicationLab03.CachForlders;
using WebApplicationLab03.Tables;

namespace WebApplicationLab03
{
    public class Startup
    {
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Map("/users", PrintCachedUsersService);
            app.Map("/notetags", PrintCachedNoteTagsService);
            app.Map("/notestatuses", PrintCachedNoteStatusesService);
            app.Map("/notes", PrintCachedNotesService);
            app.Map("/subnotes", PrintCachedSubNotesService);
            app.Map("/info", InfoPage);
            app.Map("/searchform1", SearchForm1);
            app.Map("/GetForm1Result", GetForm1Result);
            app.UseSession();
            app.Map("/searchform2", SearchForm2);
            app.Map("/GetForm2Result", GetForm2Result);


            app.Run(async (context) =>
            {
                string HtmlString = "<a href=\"/users\">Users</a>" + "<br>" +
                "<a href=\"/notetags\">Tags</a>" + "<br>" +
                "<a href=\"/notestatuses\">NoteStatuses</a>" + "<br>" +
                "<a href=\"/notes\">Notes</a>" + "<br>" +
                "<a href=\"/subnotes\">SubNotes</a>" + "<br>" +
                "<a href=\"/info\">Info</a>" + "<br>" +
                "</BODY></HTML>";

                await context.Response.WriteAsync(HtmlString);

            });

        }
        public void ConfigureServices(IServiceCollection services)
        {
            // внедрение зависимости для доступа к БД с использованием EF
            string connection = "Server=CALMBL4;Database=NotePlannerDB;Trusted_Connection=True;";
            services.AddDbContext<NotePlannerDbContext>(options => options.UseSqlServer(connection));
            // внедрение зависимости CachedTanksService
            services.AddTransient<CachedNotesService>();
            services.AddTransient<CachedNoteStatusesService>();
            services.AddTransient<CachedNoteTagsService>();
            services.AddTransient<CachedSubNotesService>();
            services.AddTransient<CachedUsersService>();

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMemoryCache();
        }

        private static void GetForm2Result(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                var form = context.Request.Form;
                context.Session.SetInt32("noteId", Convert.ToInt32(form["noteId"]));
                context.Session.SetInt32("tagId", Convert.ToInt32(form["tags"]));

                CachedNotesService cachedNotesService = context.RequestServices.GetService<CachedNotesService>();
                List<Note> notes = cachedNotesService.GetNotes().Where(t => t.NoteId > context.Session.GetInt32("noteId") && t.TagId == context.Session.GetInt32("tagId")).Take(20).ToList();

                if (notes != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    foreach (var note in notes)
                    {
                        stringBuilder.Append($"ID = {note.NoteId} Note {note.Title} NoteDescription {note.NoteDescription} UserID {note.UserId} TagID {note.TagId} NoteStatusID {note.NoteStatusId} StartTime {note.StartTime} EndTime {note.EndTime} CreateDate {note.CreateDate}" + Environment.NewLine);
                    }
                    return context.Response.WriteAsync(stringBuilder.ToString());
                }
                return context.Response.WriteAsync("Notes is empty...");

            });
        }
        private static void SearchForm2(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                CachedNoteTagsService cachedNoteStatusesService = context.RequestServices.GetService<CachedNoteTagsService>();
                List<NoteTag> noteTags = cachedNoteStatusesService.GetNoteTags().ToList();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<html><meta charset =\"UTF-8\"><body>Searching in Notes");
                if (context.Session.Keys.Contains("noteId") && context.Session.Keys.Contains("tagId"))
                {
                    int? numberOfNote = context.Session.GetInt32("noteId");
                    stringBuilder.Append("<br><form method=\"post\" action = \"GetForm2Result\"> Min number of Note:<br><input type = 'number' name = 'noteId' value = " + numberOfNote + ">");
                    stringBuilder.Append("<br>Тэги<br>");
                    stringBuilder.Append("<select name='tags'>");
                    foreach (var noteTag in noteTags)
                    {
                        if (noteTag.TagId == context.Session.GetInt32("tagId"))
                        {
                            stringBuilder.Append($"option value={noteTag.TagId} selected> {noteTag.TagName}</option>");
                        }
                        else
                        {
                            stringBuilder.Append($"<option value={noteTag.TagId}>{noteTag.TagName}</option>");
                        }
                    }
                    stringBuilder.Append("</select><br><input type = 'submit' value = 'Submit'></form></body></html>");
                }
                else
                {
                    stringBuilder.Append("<br><form method=\"post\" action = \"GetForm2Result\"> Min number of Note:<br><input type = 'number' name = 'noteId'>");
                    stringBuilder.Append("<br>Тэги<br>");
                    stringBuilder.Append("<select name='tags'>");
                    foreach (var noteTag in noteTags)
                    {
                        stringBuilder.Append($"<option value={noteTag.TagId}>{noteTag.TagName}</option>");
                    }
                    stringBuilder.Append("</select><br><input type = 'submit' value = 'Submit'></form></body></html>");
                }

                return context.Response.WriteAsync(stringBuilder.ToString()); 
            });
        }

        private static void GetForm1Result(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                var form = context.Request.Form;
                context.Response.Cookies.Append("noteId", form["noteId"]);
                context.Response.Cookies.Append("statuses", form["statuses"]);

                CachedNotesService cachedNotesService = context.RequestServices.GetService<CachedNotesService>();
                List<Note> notes = cachedNotesService.GetNotes().Where(t => t.NoteId > Convert.ToInt32(form["noteId"]) && t.TagId == Convert.ToInt32(form["statuses"])).Take(20).ToList();

                if (notes != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    foreach (var note in notes)
                    {
                        stringBuilder.Append($"ID = {note.NoteId} Note {note.Title} NoteDescription {note.NoteDescription} UserID {note.UserId} TagID {note.TagId} NoteStatusID {note.NoteStatusId} StartTime {note.StartTime} EndTime {note.EndTime} CreateDate {note.CreateDate}" + Environment.NewLine);
                    }
                    return context.Response.WriteAsync(stringBuilder.ToString());
                }
                return context.Response.WriteAsync("Notes is empty...");

            });
        }

        public static void SearchForm1(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("<html><meta charset=\"UTF-8\"><body>Searching in Notes");

                CachedNoteStatusesService statusService = context.RequestServices.GetService<CachedNoteStatusesService>();
                List<NoteStatus> statuses = statusService.GetNoteStatuses().ToList();

                if (context.Request.Cookies.ContainsKey("noteId") && context.Request.Cookies.ContainsKey("noteTagId"))
                {
                    int noteId = Convert.ToInt32(context.Request.Cookies["noteId"]);
                    strBuilder.Append("<br><form method=\"post\" action = \"GetForm1Result\"> Min NoteID:<br><input type = 'number' name = 'noteId' value = " + noteId + ">");

                    strBuilder.Append("<br>Пользователи<br>");
                    strBuilder.Append("<select name='statuses'>");

                    foreach (var status in statuses)
                    {
                        if (status.NoteStatusId == Convert.ToInt32(context.Request.Cookies["noteStatusId"]))
                        {
                            strBuilder.Append($"<option value={status.NoteStatusId} selected>{status.StatusName}</option>");
                        }
                        else
                        {
                            strBuilder.Append($"<option value={status.NoteStatusId}>{status.StatusName}</option>");
                        }
                    }

                    strBuilder.Append("</select><br><input type = 'submit' value = 'Submit'></form></body></html>");

                }
                else
                {
                    strBuilder.Append("<br><form method=\"post\" action = \"GetForm1Result\"> Min NoteId:<br><input type = 'number' name = 'noteId'" + ">");

                    strBuilder.Append("<br>Статусы<br>");
                    strBuilder.Append("<select name='statuses'>");

                    foreach (var status in statuses)
                    {
                        strBuilder.Append($"<option value={status.NoteStatusId}>{status.StatusName}</option>");
                    }
                    strBuilder.Append("</select><br><input type = 'submit' value = 'Submit'></form></body></html>");
                }

                return context.Response.WriteAsync(strBuilder.ToString());
            });
        }



        public static void PrintCachedUsersService(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                CachedUsersService cachedUsersService = context.RequestServices.GetService<CachedUsersService>();
                IEnumerable<User> users = cachedUsersService.GetUsers("Users20");
                string HtmlString = "<HTML><HEAD>" +
                "<TITLE>Пользователи</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                "<BODY><H1>Список пользователей</H1>" +
                "<TABLE BORDER=1>";
                HtmlString += "<TH>";
                HtmlString += "<TD>ID</TD>";
                HtmlString += "<TD>Логин</TD>";
                HtmlString += "<TD>Пороль</TD>";
                HtmlString += "<TD>Имя</TD>";
                HtmlString += "<TD>Фамилия</TD>";
                HtmlString += "<TD>Почта</TD>";
                HtmlString += "</TH>";
                foreach (var user in users)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + "</TD>";
                    HtmlString += "<TD>" + user.UserId + "</TD>";
                    HtmlString += "<TD>" + user.UserName + "</TD>";
                    HtmlString += "<TD>" + user.UserPassword + "</TD>";
                    HtmlString += "<TD>" + user.FirstName + "</TD>";
                    HtmlString += "<TD>" + user.LastName + "</TD>";
                    HtmlString += "<TD>" + user.Email + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>" +
                "<a href=\"/notetags\">Перейти к тэгам</a>" + "<br>" +
                "<a href=\"/notestatuses\">Перейти к стастусам</a>" + "<br>" +
                "<a href=\"/notes\">Перейти к заметкам</a>" + "<br>" +
                "<a href=\"/subnotes\">Перейти к подзаметкам</a>" + "<br>" +
                "<a href=\"/info\">Перейти к INFO</a>" + "<br>" +
                "</BODY></HTML>";
                return context.Response.WriteAsync(HtmlString);
            });
        }

        public static void PrintCachedNoteTagsService(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                CachedNoteTagsService cachedNoteTagsService = context.RequestServices.GetService<CachedNoteTagsService>();
                IEnumerable<NoteTag> noteTags = cachedNoteTagsService.GetNoteTags("NoteTags20");
                string HtmlString = "<HTML><HEAD>" +
                "<TITLE>Тэги заметок</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                "<BODY><H1>Список тэгов</H1>" +
                "<TABLE BORDER=1>";
                HtmlString += "<TH>";
                HtmlString += "<TD>ID</TD>";
                HtmlString += "<TD>Название</TD>";
                HtmlString += "</TH>";
                foreach (var noteTag in noteTags)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + "</TD>";
                    HtmlString += "<TD>" + noteTag.TagId + "</TD>";
                    HtmlString += "<TD>" + noteTag.TagName + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>" +
                "<a href=\"/users\">Перейти к пользователям</a>" + "<br>" +
                "<a href=\"/notestatuses\">Перейти к стастусам</a>" + "<br>" +
                "<a href=\"/notes\">Перейти к заметкам</a>" + "<br>" +
                "<a href=\"/subnotes\">Перейти к подзаметкам</a>" + "<br>" +
                "<a href=\"/info\">Перейти к INFO</a>" + "<br>" +
                "</BODY></HTML>";
                return context.Response.WriteAsync(HtmlString);
            });
        }

        public static void PrintCachedNoteStatusesService(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                CachedNoteStatusesService cachedNoteStatusesService = context.RequestServices.GetService<CachedNoteStatusesService>();
                IEnumerable<NoteStatus> noteStatuses = cachedNoteStatusesService.GetNoteStatuses("NoteStatuses20");
                string HtmlString = "<HTML><HEAD>" +
                "<TITLE>Статусы заметок</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                "<BODY><H1>Список статусов</H1>" +
                "<TABLE BORDER=1>";
                HtmlString += "<TH>";
                HtmlString += "<TD>ID</TD>";
                HtmlString += "<TD>Название</TD>";
                HtmlString += "</TH>";
                foreach (var noteStatus in noteStatuses)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + "</TD>";
                    HtmlString += "<TD>" + noteStatus.NoteStatusId + "</TD>";
                    HtmlString += "<TD>" + noteStatus.StatusName + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>" +
                "<a href=\"/users\">Перейти к пользователям</a>" + "<br>" +
                "<a href=\"/notetags\">Перейти к тэгам</a>" + "<br>" +
                "<a href=\"/notes\">Перейти к заметкам</a>" + "<br>" +
                "<a href=\"/subnotes\">Перейти к подзаметкам</a>" + "<br>" +
                "<a href=\"/info\">Перейти к INFO</a>" + "<br>" +
                "</BODY></HTML>";
                return context.Response.WriteAsync(HtmlString);
            });
        }

        public static void PrintCachedNotesService(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                CachedNotesService cachedNotesService = context.RequestServices.GetService<CachedNotesService>();
                IEnumerable<Note> notes = cachedNotesService.GetNotes("Notes20");
                string HtmlString = "<HTML><HEAD>" +
                "<TITLE>Заметки</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                "<BODY><H1>Список заметок</H1>" +
                "<TABLE BORDER=1>";
                HtmlString += "<TH>";
                HtmlString += "<TD>ID</TD>";
                HtmlString += "<TD>Название</TD>";
                HtmlString += "<TD>Описание</TD>";
                HtmlString += "<TD>ID Пользователя</TD>";
                HtmlString += "<TD>ID Тэга</TD>";
                HtmlString += "<TD>ID Статуса</TD>";
                HtmlString += "<TD>Дата начала</TD>";
                HtmlString += "<TD>Дата окончания</TD>";
                HtmlString += "<TD>Дата создания</TD>";
                HtmlString += "</TH>";
                foreach (var note in notes)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + "</TD>";
                    HtmlString += "<TD>" + note.NoteId + "</TD>";
                    HtmlString += "<TD>" + note.Title + "</TD>";
                    HtmlString += "<TD>" + note.NoteDescription + "</TD>";
                    HtmlString += "<TD>" + note.UserId + "</TD>";
                    HtmlString += "<TD>" + note.TagId + "</TD>";
                    HtmlString += "<TD>" + note.NoteStatusId + "</TD>";
                    HtmlString += "<TD>" + note.StartTime + "</TD>";
                    HtmlString += "<TD>" + note.EndTime + "</TD>";
                    HtmlString += "<TD>" + note.CreateDate + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>" +
                "<a href=\"/users\">Перейти к пользователям</a>" + "<br>" +
                "<a href=\"/notetags\">Перейти к тэгам</a>" + "<br>" +
                "<a href=\"/notestatuses\">Перейти к стастусам</a>" + "<br>" +
                "<a href=\"/subnotes\">Перейти к подзаметкам</a>" + "<br>" +
                "<a href=\"/info\">Перейти к INFO</a>" + "<br>" +
                "</BODY></HTML>";
                return context.Response.WriteAsync(HtmlString);
            });
        }
        public static void PrintCachedSubNotesService(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                CachedSubNotesService cachedSubNotesService = context.RequestServices.GetService<CachedSubNotesService>();
                IEnumerable<SubNote> subNotes = cachedSubNotesService.GetSubNotes("SubNotes20");
                string HtmlString = "<HTML><HEAD>" +
                "<TITLE>Подзаметки</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                "<BODY><H1>Список подзаметок</H1>" +
                "<TABLE BORDER=1>";
                HtmlString += "<TH>";
                HtmlString += "<TD>ID</TD>";
                HtmlString += "<TD>NoteID</TD>";
                HtmlString += "<TD>Описание</TD>";
                HtmlString += "<TD>Результат выполнения</TD>";
                HtmlString += "</TH>";
                foreach (var subNote in subNotes)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + "</TD>";
                    HtmlString += "<TD>" + subNote.SubNoteId + "</TD>";
                    HtmlString += "<TD>" + subNote.NoteId + "</TD>";
                    HtmlString += "<TD>" + subNote.SubNoteDescription + "</TD>";
                    HtmlString += "<TD>" + subNote.IsCompleted + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>" +
                "<a href=\"/users\">Перейти к пользователям</a>" + "<br>" +
                "<a href=\"/notetags\">Перейти к тэгам</a>" + "<br>" +
                "<a href=\"/notestatuses\">Перейти к стастусам</a>" + "<br>" +
                "<a href=\"/notes\">Перейти к ызаметкам</a>" + "<br>" +
                "<a href=\"/info\">Перейти к INFO</a>" + "<br>" +
                "</BODY></HTML>";
                return context.Response.WriteAsync(HtmlString);
            });
        }

        public static async void InfoPage(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("CLIENT IP-ADRESS:" + context.Connection.RemoteIpAddress + "\n");
                await context.Response.WriteAsync("User-Agent: " + context.Request.Headers["User-Agent"] + "\n");
                await context.Response.WriteAsync("CLIENT PORT: " + context.Connection.RemotePort + "\n");
                await context.Response.WriteAsync("CLIENT RESPONSE URL: " + context.Request.Headers["Referer"] + "\n");
                foreach (var cookie in context.Request.Cookies)
                {
                    await context.Response.WriteAsync($"Cook: {cookie.Key} = {cookie.Value}");
                }
                return; //Выохд из конвейера
            });
        }
    }
}
