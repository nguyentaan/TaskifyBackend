using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;

namespace TaskManagementAPI.Services
{
    public class TaskReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public TaskReminderService(IServiceScopeFactory scopeFactory)
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
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Adjust as needed
            }
        }

        private async Task CheckForUpComingTasks()
        {
            using var scope = _scopeFactory.CreateScope();
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            // Find tasks due within the next 24 hours
            var upcomingTasks = await applicationDbContext.Tasks
                .Include(t => t.User)
                .Where(t => !t.IsCompleted && !t.ReminderSent && t.DueDate <= DateTime.Now.AddDays(1))
                .ToListAsync();


            foreach (var task in upcomingTasks)
            {
                if (task.User != null && !string.IsNullOrEmpty(task.User.Email))
                {
                    await emailService.SendReminderEmail(task.User.Email, task.Title, task.DueDate);
                    task.ReminderSent = true;
                }
            }

            await applicationDbContext.SaveChangesAsync();
        }
    }
}
