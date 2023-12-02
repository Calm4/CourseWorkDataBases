using static System.Net.Mime.MediaTypeNames;
using Lab_05.Models;

namespace Lab_05.ViewModels
{
    public class TakeNoteViewModel
    {
        public Note Note { get; set; }

        public int NoteId { get; set; }
    }
}
