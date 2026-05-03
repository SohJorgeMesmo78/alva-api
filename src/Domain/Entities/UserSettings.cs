using System;

namespace Domain.Entities
{
    public class UserSettings
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        
        // Navigation property
        public User User { get; set; } = null!;

        // Preferences
        public string Language { get; set; } = "pt-BR";
        public string Country { get; set; } = "BR";
        public string Timezone { get; set; } = "America/Sao_Paulo";
        public string DateFormat { get; set; } = "dd/MM/yyyy";
        public string TimeFormat { get; set; } = "24h";
        public string Theme { get; set; } = "dark";
        public string PrimaryColor { get; set; } = "#FFAA00";
        public string StartOfWeek { get; set; } = "0"; // 0 = Domingo
        public bool NotificationsEnabled { get; set; } = true;
        public bool SoundEnabled { get; set; } = true;
    }
}
