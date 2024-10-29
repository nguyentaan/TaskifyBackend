using Microsoft.AspNetCore.Identity;

namespace TaskManagementAPI.Models
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
