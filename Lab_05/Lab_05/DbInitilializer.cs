using Lab_05.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;

namespace Lab_05
{
    public class DbInitilializer
    {

        private const int NOTE_STATUSES_COUNT = 4;
        private const int NOTE_TAGS_COUNT = 5;
        private const int USERS_COUNT = 500;
        private const int NOTES_COUNT = 5000;
        private const int SUB_NOTES_COUNT = 20000;

        //Statuses
        private static string[] statusesVocabulary = new string[NOTE_STATUSES_COUNT] { "Запланировано", "Выполняется", "Завершено", "Отложено" };

        //Tags
        private static string[] tagsVocabulary = new string[NOTE_TAGS_COUNT] { "Игры", "Фильмы", "Программирование", "Спорт", "Покупки" };

        // Users
        private static string[] loginsVocabulary = { "kirieshka", "chelik", "mishutka", "pusya", "kingboss", "trisinaBOX" };
        private static string[] passwordsVocabulary = { "password", "pass_W_ord", "secret", "pusya", "porolchik", "passssw_" };
        private static string[] emailVocabulary = { "boss.roto", "kesik.sos", "tuzik_puzo", "pupchanskiy", "trybochist", "kosvos", "alt_f4", "ctrl_atl_del" };
        private static string[] firstNameVocabulary = { "Данила_", "Кирилл_", "Сергей_", "Никита_", "Антон_", "Дмитрий_", "Иван_", "Максим_", "Богдан_", "Артем_" };
        private static string[] secondNameVocabulary = { "Буякевич_", "Громыко_", "Игнатенков_", "Гаращук_", "Ешман_", "Воянец_", "Кононович_", "Щербенко_", "Яковлев_", "Ленин_", "Сталин_", "Хрущев_" };

        //SubNotes
        private static int noteIdInSubNotes;
        private static string subNoteDescription = "SubNoteDescription_";
        private static bool isCompleted = true;


        //Notes
        private static string noteTitle = "NoteTitle_";
        private static string noteDescription = "NoteDescription_";
        private int userID;
        private int subNoteID;
        private int tagID;

        private static DateTime startTime;
        private static DateTime endTime;
        private static DateTime creatTime;

        public static void Initialize(NotePlannerDbContext db, IServiceProvider serviceProvider)
        {


            Random randomObj = new Random(1);
            db.Database.EnsureCreated();


            if (db.SubNotes.Any())
            {
                return;
            }

            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //NoteStatuses
            foreach (var noteStatus in statusesVocabulary)
            {
                db.NoteStatuses.Add(new NoteStatus { StatusName = noteStatus });
            }
            db.SaveChanges();

            //NoteTags
            foreach (var noteTag in tagsVocabulary)
            {
                db.NoteTags.Add(new NoteTag { TagName = noteTag });
            }
            db.SaveChanges();


            //Users
            /*for (int i = 0; i < USERS_COUNT; i++)
            {
                db.Users.Add(new User
                {
                    UserName = loginsVocabulary[randomObj.Next(passwordsVocabulary.Length)] + i,
                    UserPassword = passwordsVocabulary[randomObj.Next(passwordsVocabulary.Length)] + i,
                    Email = emailVocabulary[randomObj.Next(emailVocabulary.Length)] + i + "@mail.ru",
                    FirstName = firstNameVocabulary[randomObj.Next(firstNameVocabulary.Length)] + i,
                    LastName = secondNameVocabulary[randomObj.Next(secondNameVocabulary.Length)] + i,
                });
            }
            db.SaveChanges();*/

            //Notes
            for (int i = 0; i < NOTES_COUNT; i++)
            {

                int userId = randomObj.Next(0, USERS_COUNT);
                db.Notes.Add(new Note
                {
                    Title = noteTitle + i,
                    NoteDescription = noteDescription + i,
                    UserId = userManager.Users.ToList()[userId].Id,
                    NoteTagId = randomObj.Next(1, NOTE_TAGS_COUNT + 1),
                    NoteStatusId = randomObj.Next(1, NOTE_STATUSES_COUNT + 1),
                    StartTime = DateTime.Now.AddDays(randomObj.Next(1, 60)),
                    EndTime = DateTime.Now.AddDays(randomObj.Next(60, 120)),
                    CreateDate = DateTime.Now.AddDays(randomObj.Next(-10, -1)),
                });
                db.SaveChanges();
            }


            //SubNotes
            for (int i = 0; i < SUB_NOTES_COUNT; i++)
            {
                isCompleted = !isCompleted;
                db.SubNotes.Add(new SubNote
                {
                    NoteId = randomObj.Next(1, NOTES_COUNT),
                    SubNoteDescription = subNoteDescription + i,
                    IsCompleted = isCompleted
                });
            }
            db.SaveChanges();
        }

        private static int RandomNumber(int minNum, int maxNum)
        {
            Random random = new Random();
            return random.Next(minNum, maxNum);
        }
    }
}
