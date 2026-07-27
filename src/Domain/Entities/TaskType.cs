using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class TaskType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // appointment, habit, deadline_goal
        public string Description { get; set; } = string.Empty;

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
