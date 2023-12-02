using Lab_05.Models;
using Microsoft.AspNetCore.Identity;

namespace Lab_05.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }    
        public string Surname { get; set; }

        public virtual ICollection<Note> Results { get; set; } = new List<Note>();
    }
}
