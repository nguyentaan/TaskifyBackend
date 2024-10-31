using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            var task = await _taskService.CreateTaskAsync(createTaskDto);
            return Ok(new { message = "Task created successfully", task });
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetTasksByUserId(string userId)
        {
            var tasks = await _taskService.GetTasksByUserIdAsync(userId);
            if (!tasks.Any()) return NotFound(new { message = "No tasks found for this user" });

            return Ok(tasks);
        }

        [HttpPut("update/{taskId}")]
        public async Task<IActionResult> UpdateTaskById(int taskId, [FromBody] UpdateTaskDto updateTaskDto)
        {
            var updatedTask = await _taskService.UpdateTaskAsync(taskId, updateTaskDto);
            if (updatedTask == null) return NotFound(new { message = "Task not found" });

            return Ok(new { message = "Task updated successfully", task = updatedTask });
        }

        [HttpPut("{taskId}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int taskId, [FromBody] TaskStatusUpdateDto statusUpdate)
        {
            var updatedTask = await _taskService.UpdateTaskStatusAsync(taskId, statusUpdate.IsCompleted);
            if (updatedTask == null) return NotFound(new { message = "Task not found" });

            return Ok(new { message = "Task status updated successfully", task = updatedTask });
        }


        [HttpDelete("delete/{taskId}")]
        public async Task<IActionResult> DeleteTaskById(int taskId)
        {
            var isDeleted = await _taskService.DeleteTaskByIdAsync(taskId);
            if (!isDeleted) return NotFound(new { message = "Task not found" });

            return Ok(new { message = "Task deleted successfully" });
        }
    }
}
