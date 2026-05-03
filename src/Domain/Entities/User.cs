using System;

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
    }
}
