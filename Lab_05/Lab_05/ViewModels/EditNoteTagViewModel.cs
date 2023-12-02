namespace Lab_05.ViewModels;

public class EditNoteTagViewModel
{
    public int Id { get; set; }
    public string TagName { get; set; }

    public EditNoteTagViewModel()
    {
        
    }

    public EditNoteTagViewModel(string tagName)
    {
        TagName = tagName;
    }
}