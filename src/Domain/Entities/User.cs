using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        
        // Navigation property for 1:1 relationship
        public UserSettings Settings { get; set; } = null!;

        // Relation 1:N with Tasks
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
