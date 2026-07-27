using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Infrastructure.Data
{
    public class AlvaDbContext : DbContext
    {
        public AlvaDbContext(DbContextOptions<AlvaDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserSettings> UserSettings { get; set; } = null!;
        public DbSet<TaskType> TaskTypes { get; set; } = null!;
        public DbSet<TaskItem> Tasks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1:1 Relationship Configuration
            modelBuilder.Entity<User>()
                .HasOne(u => u.Settings)
                .WithOne(s => s.User)
                .HasForeignKey<UserSettings>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Task Relations
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.TaskType)
                .WithMany(type => type.Tasks)
                .HasForeignKey(t => t.TaskTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Data
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var settingsId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = userId,
                Email = "jf78pereira@gmail.com",
                Username = "SohJorgeMesmo78",
                PasswordHash = "$2a$11$ru9k7HAiVhVxH/nQrACOduyKJaaC9CNuIIXMwfMefbALhvvQxfLai" // @Senha123
            });

            modelBuilder.Entity<UserSettings>().HasData(new UserSettings
            {
                Id = settingsId,
                UserId = userId,
                Language = "pt-BR",
                Country = "BR",
                Timezone = "America/Sao_Paulo",
                DateFormat = "dd/MM/yyyy",
                TimeFormat = "24h",
                Theme = "dark",
                PrimaryColor = "#FFAA00",
                StartOfWeek = "0",
                NotificationsEnabled = true,
                SoundEnabled = true
            });

            // Seed TaskTypes
            var appointmentTypeId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var habitTypeId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var deadlineGoalTypeId = Guid.Parse("55555555-5555-5555-5555-555555555555");

            modelBuilder.Entity<TaskType>().HasData(
                new TaskType { Id = appointmentTypeId, Name = "appointment", Description = "Compromisso com data e hora" },
                new TaskType { Id = habitTypeId, Name = "habit", Description = "Hábito ou Rotina" },
                new TaskType { Id = deadlineGoalTypeId, Name = "deadline_goal", Description = "Meta com prazo" }
            );
        }
    }
}
