using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;

namespace TaskManagementAPI.Services
{
    public class TaskReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public TaskReminderService(ApplicationDbContext @object, IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task TriggerCheckForUpcomingTasks() // Public method to trigger the check
        {
            await CheckForUpComingTasks();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckForUpComingTasks();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task CheckForUpComingTasks()
        {
            using var scope = _scopeFactory.CreateScope();
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            // Find tasks due in the next day that haven't had reminders sent
            var upcomingTasks = await applicationDbContext.Tasks
                .Include(t => t.User) // Ensure that the User is included
                .Where(t => !t.IsCompleted && !t.ReminderSent && t.DueDate <= DateTime.Now.AddDays(1))
                .ToListAsync();


            foreach (var task in upcomingTasks)
            {
                if (task.User != null) // Check if User is not null
                {
                    string? userEmail = task.User.Email;

                    if (!string.IsNullOrEmpty(userEmail)) // Check if Email is not null or empty
                    {
                        await emailService.SendReminderEmail(userEmail, task.Title, task.DueDate);
                        task.ReminderSent = true;
                    }
                }
            }

            await applicationDbContext.SaveChangesAsync();
        }
    }
}
