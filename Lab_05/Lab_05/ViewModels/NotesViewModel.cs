using static System.Net.Mime.MediaTypeNames;
using Lab_05.Models;
using System.ComponentModel.DataAnnotations;

namespace Lab_05.ViewModels
{
    public class NotesViewModel
    {
        public IEnumerable<Note> Notes { get; }
        public PageViewModel PageViewModel { get; }
        public FilterNotesViewModel FilterNotesViewModel { get; }
        public ApplicationUser ApplicationUser { get; }

        public virtual IEnumerable<SubNote> SubNotes { get; set; } = new List<SubNote>();

        [DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MMMMM-dd}")]
        public DateTime StartDateTime { get; set; }
        [DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MMMMM-dd}")]
        public DateTime EndDateTime { get; set; }
        [DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MMMMM-dd}")]
        public DateTime CreateDateTime { get; set; }

        public NotesViewModel(IEnumerable<Note> notes, PageViewModel pageViewModel, FilterNotesViewModel filterNotesViewModel, ApplicationUser applicationUser, IEnumerable<SubNote> subNotes, DateTime startTime, DateTime endTime, DateTime createTime)
        {
            Notes = notes;
            PageViewModel = pageViewModel;
            FilterNotesViewModel = filterNotesViewModel;
            ApplicationUser = applicationUser;
            SubNotes = subNotes;
            StartDateTime = startTime;
            EndDateTime = endTime;
            CreateDateTime = createTime;

        }
    }
}
