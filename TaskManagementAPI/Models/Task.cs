﻿namespace TaskManagementAPI.Models
{
    public class Task
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public required string UserId { get; set; }
        public User User { get; set; }

        public bool ReminderSent { get; set; } = false;
    }
}
