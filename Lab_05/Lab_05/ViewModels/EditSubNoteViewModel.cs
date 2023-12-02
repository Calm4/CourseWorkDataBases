namespace Lab_05.ViewModels
{
    public class EditSubNoteViewModel
    {
        public int Id { get; set; }
        public int NoteId { get; set; }
        public string SubNoteDescription { get; set; }
        public bool IsCompleted { get; set; }

        public EditSubNoteViewModel()
        {
            
        }
        public EditSubNoteViewModel(int id, string subNoteDescription, bool isCompleted, int noteId)
        {
            Id = id;
            NoteId = noteId;
            SubNoteDescription = subNoteDescription;
            IsCompleted = isCompleted;
        }

    }
}
