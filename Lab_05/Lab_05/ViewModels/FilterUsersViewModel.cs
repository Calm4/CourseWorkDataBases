namespace Lab_05.ViewModels
{
    public class FilterUsersViewModel
    {
        public FilterUsersViewModel(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; }

    }
}
