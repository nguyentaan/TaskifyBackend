using Moq;
using TaskManagementAPI.Services;
using Xunit;

namespace TaskManagementAPI.UnitTest
{
    public class EmailServiceTests
    {
        [Fact]
        public async Task SendReminderEmail_SendsEmailWithCorrectParameters()
        {
            //Arrange
            var mockEmailService = new Mock<IEmailService>();
            var email = "test@example.com";
            var taskTitle = "Test Task";
            var dueDate = DateTime.Now.AddDays(1);

            //Act
            await mockEmailService.Object.SendReminderEmail(email, taskTitle, dueDate);

            //Assert
            mockEmailService.Verify(e => e.SendReminderEmail(
                email,
                taskTitle,
                dueDate),
                Times.Once);
        }
    }
}
