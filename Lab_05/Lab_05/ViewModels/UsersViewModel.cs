using Lab_05.Models;

namespace Lab_05.ViewModels
{
    public class UsersViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; }
        public PageViewModel PageViewModel { get; }
        public ApplicationUser ApplicationUser { get; }
        public FilterUsersViewModel FilterUsersViewModel { get; }

        public UsersViewModel(IEnumerable<ApplicationUser> users, PageViewModel pageViewModel
            , FilterUsersViewModel filterNotesViewModel)
        {
            Users = users;
            PageViewModel = pageViewModel;
            FilterUsersViewModel = filterNotesViewModel;
        }
    }
}

