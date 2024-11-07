using Moq;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;
using Xunit;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementAPI.UnitTest
{
    public class TaskReminderServiceTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public TaskReminderServiceTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TaskReminderServiceTests")
                .Options;
        }

        [Fact]
        public async System.Threading.Tasks.Task CheckForUpcomingTasks_SendsEmailForTasksDueSoon()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var user = new User { Email = "test@example.com" };
                context.Users.Add(user); // Ensure the user exists
                context.Tasks.Add(new Models.Task
                {
                    Title = "Test Task",
                    DueDate = DateTime.Now.AddHours(1), // due soon
                    IsCompleted = false,
                    ReminderSent = false,
                    UserId = user.Id, // Link to the user
                    User = user // Link to the user
                });
                await context.SaveChangesAsync();
            }

            var emailServiceMock = new Mock<IEmailService>();
            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            var scopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            // Setup the mock scope
            scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
            scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(ApplicationDbContext)))
                .Returns(new ApplicationDbContext(_options)); // Use the same in-memory context
            serviceProviderMock.Setup(x => x.GetService(typeof(IEmailService)))
                .Returns(emailServiceMock.Object);

            var taskReminderService = new TaskReminderService(scopeFactoryMock.Object);

            // Act
            await taskReminderService.TriggerCheckForUpcomingTasks();

            // Assert
            emailServiceMock.Verify(e => e.SendReminderEmail("test@example.com", "Test Task", It.IsAny<DateTime>()), Times.Once);

            // Verify the ReminderSent status
            using (var context = new ApplicationDbContext(_options))
            {
                var task = await context.Tasks.FirstOrDefaultAsync(t => t.Title == "Test Task");
                Assert.NotNull(task);
                Assert.True(task.ReminderSent); // Check that the reminder was sent
            }
        }
    }
}
