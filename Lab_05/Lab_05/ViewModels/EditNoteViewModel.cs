using Lab_05.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Lab_05.ViewModels
{
    public class EditNoteViewModel
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public int SelectedNoteStatusId { get; set; }
        public int SelectedNoteTagId { get; set; }

        [Required(ErrorMessage = "Пожалуйста, укажите дату и время начала выполнения.")]
        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessage = "Пожалуйста, укажите дату и время окончания выполнения.")]
        [DataType(DataType.DateTime)]
        public DateTime EndDateTime { get; set; }
                
        public EditNoteViewModel()
        {
            
        }

        public EditNoteViewModel(List<NoteStatus> noteStatuses, List<NoteTag> noteTags,int id,int status, int tag, string title, string description, DateTime startDateTime, DateTime endDateTime)
        {
            NoteStatuses = new SelectList(noteStatuses, "NoteStatusId", "StatusName", status);
            NoteTags = new SelectList(noteTags, "NoteTagId", "TagName", tag);
            Id = id;
            SelectedNoteStatusId = status;
            SelectedNoteTagId = tag;
            Title = title;
            Description = description;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }
        public IEnumerable<SelectListItem>? NoteStatuses { get; set; }
        public IEnumerable<SelectListItem>? NoteTags { get; set; }

    }
}
