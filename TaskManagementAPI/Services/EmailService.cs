using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace TaskManagementAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendReminderEmail(string toEmail, string taskTitle, DateTime dueDate)
        {
            var subject = $"Reminder: Task '{taskTitle}' Due Soon";
            var body = $"Your task '{taskTitle}' is due on {dueDate:G}. Please take necessary action.";

            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var appPassword = _configuration["EmailSettings:AppPassword"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = _configuration["EmailSettings:SmtpPort"];

            // Send email using SMTP client
            using var client = new SmtpClient(smtpServer)
            {
                Credentials = new NetworkCredential(fromEmail, appPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(fromEmail, toEmail, subject, body);
            await client.SendMailAsync(mailMessage);
        }
    }
}
