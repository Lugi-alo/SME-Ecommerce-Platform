using Microsoft.AspNetCore.Identity;

namespace Dissertation.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email {get; set; }
        public string UserType { get; set; }

    }
}
