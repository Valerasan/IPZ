using Microsoft.AspNetCore.Identity;

namespace IPZ_1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

    }
}
