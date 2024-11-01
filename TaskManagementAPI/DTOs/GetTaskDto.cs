using TaskManagementAPI.Models;

namespace TaskManagementAPI.DTOs
{
    public class GetTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public string UserId { get; set; }
        public string Email{ get; set; }
    }
}
