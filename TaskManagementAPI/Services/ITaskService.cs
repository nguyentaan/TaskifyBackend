using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Services
{
    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(CreateTaskDto task);
        Task<IEnumerable<GetTaskDto>> GetTasksByUserIdAsync(string userId);
        Task<TaskDto?> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto);
        Task<TaskDto?> UpdateTaskStatusAsync(int taskId, bool isCompleted);
        Task<bool> DeleteTaskByIdAsync(int taskId);
    }
}
