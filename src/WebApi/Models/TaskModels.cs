using System;

namespace WebApi.Models
{
    public class CreateTaskDto
    {
        public string Type { get; set; } = string.Empty; // appointment, habit, deadline_goal
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime ScheduledFor { get; set; }
        
        // Extended fields for Habit & Goal
        public DateTime? StartDate { get; set; }
        public DateTime? Deadline { get; set; }
        public int? TargetVolume { get; set; }
        public string? Unit { get; set; }
    }

    public class TaskItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool Completed { get; set; }
        
        // Appointment specific
        public string? Time { get; set; }
        public string? Location { get; set; }

        // Habit specific
        public int? Streak { get; set; }

        // Goal specific
        public DateTime? Deadline { get; set; }
        public int? TargetVolume { get; set; }
        public int? CurrentVolume { get; set; }
        public string? Unit { get; set; }
    }
}
