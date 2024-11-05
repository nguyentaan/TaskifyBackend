using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskReminderController : Controller
    {
        private readonly TaskReminderService _taskReminderService;
        public TaskReminderController(TaskReminderService taskReminderService)
        {
            _taskReminderService = taskReminderService;
        }
        [HttpPost("trigger-reminder-check")]
        public async Task<IActionResult> TriggerReminderCheck()
        {
            await _taskReminderService.TriggerCheckForUpcomingTasks();
            return Ok(new { message = "Reminder check triggered" });
        }
    }
}
