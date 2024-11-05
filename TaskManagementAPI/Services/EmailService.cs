using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

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
            var smtpPort = 587;

            //create the email message
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Task Reminder", fromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart("plain") { Text = body };

            //Send the email
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(smtpServer, smtpPort, false);
                await client.AuthenticateAsync(fromEmail, appPassword);
                await client.SendAsync(email);
            }
            finally
            {

               await client.DisconnectAsync(true);
            }
        }
    }
}
