using Lab_05.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab_05.ViewModels
{
    public class FilterNotesViewModel
    {
        public FilterNotesViewModel(List<NoteStatus> noteStatuses, List<NoteTag> noteTags, int status, int tag, string title, DateTime startData, DateTime endData, DateTime createData)
        {
            NoteStatuses = new SelectList(noteStatuses, "NoteStatusId", "StatusName", status);
            NoteTags = new SelectList(noteTags,"NoteTagId","TagName", tag);
            SelectedStatus = status;
            SelectedTag = tag;
            SelectedTitle = title;
            SelectedStartData = startData;
            SelectedEndData = endData;
            SelectedCreateData = createData;
        }

        public SelectList NoteStatuses { get; }
        public SelectList NoteTags { get; }

        public int SelectedStatus { get; }
        public int SelectedTag { get; }
        public string SelectedTitle { get; }

        public DateTime SelectedStartData { get; }
        public DateTime SelectedEndData { get; }
        public DateTime SelectedCreateData { get; }

    }
}
