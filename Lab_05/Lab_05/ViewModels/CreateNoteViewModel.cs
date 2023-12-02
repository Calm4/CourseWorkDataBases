using Lab_05.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab_05.ViewModels
{
    public class CreateNoteViewModel
    {
        [Required]
        [DisplayName("Название")]
        public string Title { get; set; }
        public string Description { get; set; }
        public int NoteStatusId { get; set; }

        public int NoteTagId { get; set; }
        [DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime StartDateTime { get; set; }
        [DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime EndDateTime { get; set; }

       

        public CreateNoteViewModel()
        {

        }

        public CreateNoteViewModel(List<NoteStatus> noteStatuses, List<NoteTag> noteTags)
        {
            NoteStatuses = new SelectList(noteStatuses, "NoteStatusId", "StatusName");
            NoteTags = new SelectList(noteTags, "NoteTagId", "TagName");

        }
        public IEnumerable<SelectListItem>? NoteStatuses { get; set; }
        public IEnumerable<SelectListItem>? NoteTags { get; set; }
    }
}
