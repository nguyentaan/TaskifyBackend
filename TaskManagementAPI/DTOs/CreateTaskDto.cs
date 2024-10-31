namespace TaskManagementAPI.DTOs
{
    public class CreateTaskDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public required string UserId { get; set; }
    }
}
