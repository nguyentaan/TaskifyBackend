namespace TaskManagementAPI.Services
{
    public interface IEmailService
    {
        Task SendReminderEmail(string toEmail, string taskTitle, DateTime dueDate);
    }
}
