using Lab_05.Models;

namespace Lab_05.ViewModels
{
    public class NoteTagsViewModel
    {
        public IEnumerable<NoteTag> Tags { get; set; }
        public string TagName { get; set; }

        public NoteTagsViewModel(IEnumerable<NoteTag> noteTags, string tagName)
        {
            Tags = noteTags;
            TagName = tagName;

        }
    }
}
