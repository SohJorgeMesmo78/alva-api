using Domain.Entities;

namespace WebApi.Models
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserSettingsDto Settings { get; set; } = null!;
    }

    public class UserSettingsDto
    {
        public string Language { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Timezone { get; set; } = string.Empty;
        public string DateFormat { get; set; } = string.Empty;
        public string TimeFormat { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        public string PrimaryColor { get; set; } = string.Empty;
        public string StartOfWeek { get; set; } = string.Empty;
        public bool NotificationsEnabled { get; set; }
        public bool SoundEnabled { get; set; }

        public static UserSettingsDto FromEntity(UserSettings entity)
        {
            return new UserSettingsDto
            {
                Language = entity.Language,
                Country = entity.Country,
                Timezone = entity.Timezone,
                DateFormat = entity.DateFormat,
                TimeFormat = entity.TimeFormat,
                Theme = entity.Theme,
                PrimaryColor = entity.PrimaryColor,
                StartOfWeek = entity.StartOfWeek,
                NotificationsEnabled = entity.NotificationsEnabled,
                SoundEnabled = entity.SoundEnabled
            };
        }
    }
}
