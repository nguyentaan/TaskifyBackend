using TaskManagementAPI.Data;
using TaskManagementAPI.DTOs;
using TaskEntity = TaskManagementAPI.Models.Task;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        public TaskService(ApplicationDbContext context)
        {
               _context = context;
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            var task = new TaskEntity
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                DueDate = createTaskDto.DueDate,
                UserId = createTaskDto.UserId,
                IsCompleted = false
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                DueDate = task.DueDate,
                UserId = task.UserId
            };
        }
        public async Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(string userId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    DueDate = t.DueDate,
                    UserId = t.UserId
                })
                .ToListAsync();

            return tasks;
        }
        public async Task<TaskDto?> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return null;

            // Only update fields that are provided
            if (updateTaskDto.Title != null) task.Title = updateTaskDto.Title;
            if (updateTaskDto.Description != null) task.Description = updateTaskDto.Description;
            if (updateTaskDto.DueDate.HasValue) task.DueDate = updateTaskDto.DueDate.Value; // Cast with .Value
            if (updateTaskDto.IsCompleted.HasValue) task.IsCompleted = updateTaskDto.IsCompleted.Value;

            await _context.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                DueDate = task.DueDate,
                UserId = task.UserId
            };
        }

        public async Task<TaskDto?> UpdateTaskStatusAsync(int taskId, bool isCompleted)
        {

           var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return null;

            task.IsCompleted = isCompleted;
            await _context.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                DueDate = task.DueDate,
                UserId = task.UserId
            };
        }

        public async Task<bool> DeleteTaskByIdAsync(int taskId)
        {

           var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
