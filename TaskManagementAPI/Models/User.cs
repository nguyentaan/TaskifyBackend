using Microsoft.AspNetCore.Identity;

namespace TaskManagementAPI.Models
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }

        // Navigation property for related tasks
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
