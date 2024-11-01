using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Models.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure User entity
            builder.Entity<User>()
                .Property(u => u.Initials)
                .HasMaxLength(5);

            // Configure default schema for Identity
            builder.HasDefaultSchema("identity");

            // Configure the Task entity to User relationship
            builder.Entity<Models.Task>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks) // Ensure the navigation property is specified
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: specify delete behavior
        }
    }
}
