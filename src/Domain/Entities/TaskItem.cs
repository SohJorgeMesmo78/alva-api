using System;

namespace Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        
        // Relacionamentos
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public Guid TaskTypeId { get; set; }
        public TaskType TaskType { get; set; } = null!;

        // Campos Base
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; } // Data do compromisso ou deadline
        public bool Completed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Específicos de Appointment (Compromisso)
        public string? Time { get; set; } // e.g. "19:00"
        public string? Location { get; set; }

        // Específicos de Habit (Hábito)
        public int? Streak { get; set; }

        // Específicos de DeadlineGoal (Meta)
        public DateTime? Deadline { get; set; }
        public int? TargetVolume { get; set; }
        public int? CurrentVolume { get; set; }
        public string? Unit { get; set; }
    }
}
