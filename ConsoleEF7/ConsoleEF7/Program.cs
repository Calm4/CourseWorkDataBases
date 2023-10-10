using ConsoleEF7;
using ConsoleEF7.NotePlannerDB;
using Microsoft.EntityFrameworkCore;
using System;

Console.ForegroundColor = ConsoleColor.White;
while (true)
{
    Console.WriteLine("\t\t\t================МЕНЮ================");
    Console.WriteLine("Задание 1: Выборку всех данных из таблицы, стоящей в схеме базы данных нас стороне отношения «один»");
    Console.WriteLine("Задание 2: Выборку данных из таблицы, отфильтрованные по определенному условию, налагающему ограничения на одно или несколько полей");
    Console.WriteLine("Задание 3: Выборку данных, сгруппированных по любому из полей данных с выводом какого-либо итогового результата (min, max, avg, сount или др.) по выбранному полю из таблицы,");
    Console.WriteLine("Задание 4: Выборку данных из двух полей двух таблиц, связанных между собой тношением «один-ко-многим»");
    Console.WriteLine("Задание 5: Выборку данных из двух таблиц, связанных между собой отношением «один-ко-многим» и отфильтрованным по некоторому условию, налагающемуограничения на значения одного или нескольких полей");
    Console.WriteLine("Задание 6: Вставку данных в таблицы, стоящей на стороне отношения «Один»");
    Console.WriteLine("Задание 7: Вставку данных в таблицы, стоящей на стороне отношения «Многие»");
    Console.WriteLine("Задание 8: Удаление данных из таблицы, стоящей на стороне отношения «Один»");
    Console.WriteLine("Задание 9: Удаление данных из таблицы, стоящей на стороне отношения «Многие»");
    Console.WriteLine("Задание 10: Обновление удовлетворяющих определенному условию записей в любой из таблиц базы данных");
    Console.WriteLine("Выбирите задание которое хотите увидеть: ");
    int chooseMenu = Convert.ToInt32(Console.ReadLine());
    switch (chooseMenu)
    {
        case 1:
            Task1();
            break;
        case 2:
            Task2();
            break;
        case 3:
            Task3();
            break;
        case 4:
            Task4();
            break;
        case 5:
            Task5();
            break;
        case 6:
            Task6();
            break;
        case 7:
            Task7();
            break;
        case 8:
            Task8();
            break;
        case 9:
            Task9();
            break;
        case 10:
            Task10();
            break;
        default:
            break;
    }
    Console.ReadKey();
    Console.Clear();
}


// Задание 1
void Task1()
{
    using (var context = new NotePlannerDbContext())
    {
        int userCount = 0;
        var users = context.Users.Include(u => u.Notes).ToList();

        foreach (var user in users)
        {
            if (userCount < 5)
            {

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"User: {user.UserName}");
                Console.ForegroundColor = ConsoleColor.White;

                foreach (var note in user.Notes)
                {
                    Console.WriteLine($"Note: {note.Title}");
                }
            }
            userCount++;
        }
    }
}
// Задание 2
void Task2()
{
    using (var context = new NotePlannerDbContext())
    {
        int tagValue = 1;
        string statusValue = "Выполняется";
        var filteredData = context.Notes.Include(s => s.NoteStatus).Where(n => n.TagId.Equals(tagValue) && n.NoteStatus.StatusName.Equals(statusValue)).ToList();
        foreach (var note in filteredData)
        {
            Console.WriteLine(note.Title + "---" + note.TagId + "---" + note.NoteStatus.StatusName);
        }


    }
}
void Task3()
{
    using (var context = new NotePlannerDbContext())
    {
        int counter = 0;
        var groupedData = context.SubNotes
            .GroupBy(subNote => subNote.NoteId)
            .OrderBy(groupedSubNotes => groupedSubNotes.Key)
            .Take(100)
            .Select(groupedSubNotes => new
            {
                NoteId = groupedSubNotes.Key,
                SubNoteCount = groupedSubNotes.Count()
            });

        foreach (var group in groupedData)
        {

            Console.WriteLine($"NoteId: {group.NoteId}, SubNoteCount: {group.SubNoteCount}");
            counter++;

        }
    }
}
// Задание 4
void Task4()
{
    using (var context = new NotePlannerDbContext())
    {
        var groupedData = context.Notes
            .Include(note => note.Tag)
            .Include(note => note.NoteStatus)
            .GroupBy(note => note.NoteId)
            .Take(100)
            .Select(groupedNotes => new
            {
                NoteId = groupedNotes.Key,
                groupedNotes.FirstOrDefault().Title,
                groupedNotes.FirstOrDefault().Tag.TagName,
                groupedNotes.FirstOrDefault().NoteStatus.StatusName
            });

        foreach (var group in groupedData)
        {
            Console.WriteLine($"NoteId: {group.NoteId}, NoteTitle {group.Title}, TagName: {group.TagName}, StatusName: {group.StatusName}");
        }
    }
}
// Задание 5
void Task5()
{
    using (var context = new NotePlannerDbContext())
    {
        var groupedData = context.Notes
            .Include(note => note.Tag)
            .Include(note => note.NoteStatus)
            .Where(g => g.NoteStatus.StatusName.Equals("Завершено"))
            .GroupBy(note => note.NoteId)
            .Select(groupedNotes => new
            {
                NoteId = groupedNotes.Key,
                groupedNotes.FirstOrDefault().Title,
                groupedNotes.FirstOrDefault().Tag.TagName,
                groupedNotes.FirstOrDefault().NoteStatus.StatusName
            });

        foreach (var group in groupedData)
        {
            Console.WriteLine($"NoteId: {group.NoteId}, NoteTitle {group.Title}, TagName: {group.TagName}, StatusName: {group.StatusName}");
        }
    }
}
// Задание 6
void Task6()
{
    Console.WriteLine("Write a Username:");
    string userName = Console.ReadLine();
    Console.WriteLine("Write a Password:");
    string password = Console.ReadLine();
    Console.WriteLine("Write a Email:");
    string email = Console.ReadLine();
    Console.WriteLine("Write a First Name:");
    string firstName = Console.ReadLine();
    Console.WriteLine("Write a Last Name:");
    string lastName = Console.ReadLine();
    using (var context = new NotePlannerDbContext())
    {
        var newUser = new User
        {
            UserName = userName,
            UserPassword = password,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        context.Users.Add(newUser);
        context.SaveChanges();

        var lastFiveUsers = context.Users.OrderByDescending(u => u.UserId).Take(3).ToList();

        foreach (var user in lastFiveUsers)
        {
            // Вывод информации о пользователе
            Console.WriteLine($"User ID: {user.UserId}");
            Console.WriteLine($"Username: {user.UserName}");
            Console.WriteLine($"Username: {user.UserPassword}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"First Name: {user.FirstName}");
            Console.WriteLine($"Last Name: {user.LastName}");
            Console.WriteLine();
        }
    }
}
// Задание 7
void Task7()
{
    Console.WriteLine("Write a Title:");
    string title = Console.ReadLine();
    Console.WriteLine("Write a Descriprion:");
    string desctiption = Console.ReadLine();
    Console.WriteLine("Write a UserId:");
    int userId = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("Write a TagId:");
    int tagId = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("Write a NoteStatusId:");
    int noteStatusId = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("Write a StartTime");
    DateTime startTime = Convert.ToDateTime(Console.ReadLine());
    Console.WriteLine("Write a EndTime");
    DateTime endTime = Convert.ToDateTime(Console.ReadLine());
    Console.WriteLine("Write a CreateTime");
    DateTime createTime = Convert.ToDateTime(Console.ReadLine());
    using (var context = new NotePlannerDbContext())
    {
        var newNote = new Note
        {
            Title = title,
            NoteDescription = desctiption,
            UserId = userId,
            TagId = tagId,
            NoteStatusId = noteStatusId,
            StartTime = startTime,
            EndTime = endTime,
            CreateDate = createTime

        };

        context.Notes.Add(newNote);
        context.SaveChanges();

        var lastFiveNotes = context.Notes.OrderByDescending(u => u.NoteId).Take(3).ToList();

        foreach (var note in lastFiveNotes)
        {
            // Вывод информации о пользователе
            Console.WriteLine($"Title: {note.Title}");
            Console.WriteLine($"Description: {note.NoteDescription}");
            Console.WriteLine($"UserID: {note.UserId}");
            Console.WriteLine($"TagID: {note.TagId}");
            Console.WriteLine($"NoteStatusID: {note.NoteStatusId}");
            Console.WriteLine($"StartTime: {note.StartTime}");
            Console.WriteLine($"EndTime: {note.EndTime}");
            Console.WriteLine($"CreateTime: {note.CreateDate}");
        }
    }
}

// Задание 8
void Task8()
{
    using (var context = new NotePlannerDbContext())
    {
        Console.WriteLine("Введите ID пользователя для удаления:");
        int userId = Convert.ToInt32(Console.ReadLine());
        // Найдите запись, которую нужно удалить
        var userToDelete = context.Users.FirstOrDefault(u => u.UserId == userId);

        if (userToDelete != null)
        {
            // Удалите запись
            context.Users.Remove(userToDelete);
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("Такого пользователя нет!");
        }
    }
}
// Задание 9
void Task9()
{
    using (var context = new NotePlannerDbContext())
    {
        Console.WriteLine("Введите название(title) заметки для удаления");
        string noteTitle = Console.ReadLine(); //Title 1F875000 

        var noteToDelete = context.Notes.FirstOrDefault(u => u.Title == noteTitle);

        if (noteToDelete != null)
        {
            context.Notes.Remove(noteToDelete);
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("Заметки с таким названием нету");
        }

        var lastFiveNotes = context.Notes.OrderByDescending(u => u.NoteId).Take(3).ToList();

        foreach (var note in lastFiveNotes)
        {
            // Вывод информации о пользователе
            Console.WriteLine($"NoteId: {note.NoteId}");
            Console.WriteLine($"NoteDescription: {note.NoteDescription}");
            Console.WriteLine($"UserId: {note.UserId}");
            Console.WriteLine($"TagId: {note.TagId}");
            Console.WriteLine($"NoteStatusId: {note.NoteStatusId}");
            Console.WriteLine($"StartTime: {note.StartTime}");
            Console.WriteLine($"EndTime: {note.EndTime}");
            Console.WriteLine($"CreateDate: {note.CreateDate}");
            Console.WriteLine();
        }
    }
}
// Задание 10
void Task10()
{
    using (var context = new NotePlannerDbContext())
    {
        Console.WriteLine("Введите статус");
        int statusId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Введите тэг");
        int tagId = Convert.ToInt32(Console.ReadLine());
        var filteredData = context.Notes.Where(n => n.NoteStatusId.Equals(statusId) && n.TagId.Equals(tagId)).ToList();

        foreach (var note in filteredData)
        {
            note.Title = "Новый тайтл для моей заметки";
        }

        foreach (var note in filteredData)
        {
            Console.WriteLine("Title" + note.Title);
            Console.WriteLine("Description " + note.NoteDescription);
            Console.WriteLine("StatusId " + note.NoteStatusId);
            Console.WriteLine("TagId " + note.TagId);
        }
        context.SaveChanges();

        Console.WriteLine("Записи успешно обновлены.");
    }
}