using System.Security.Claims;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly AlvaDbContext _context;

        public TasksController(AlvaDbContext context)
        {
            _context = context;
        }

        private Guid GetUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(userIdStr ?? Guid.Empty.ToString());
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] string? date)
        {
            var userId = GetUserId();

            var query = _context.Tasks
                .Include(t => t.TaskType)
                .Where(t => t.UserId == userId);

            if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out var parsedDate))
            {
                // Return tasks for the specific day
                var startOfDay = parsedDate.Date;
                var endOfDay = startOfDay.AddDays(1);
                
                query = query.Where(t => t.Date >= startOfDay && t.Date < endOfDay);
            }

            var tasks = await query.ToListAsync();

            var taskDtos = tasks.Select(t => new TaskItemDto
            {
                Id = t.Id.ToString(),
                Title = t.Title,
                Type = t.TaskType.Name,
                Date = t.Date,
                Completed = t.Completed,
                Time = t.Time,
                Location = t.Location,
                Streak = t.Streak,
                Deadline = t.Deadline,
                TargetVolume = t.TargetVolume,
                CurrentVolume = t.CurrentVolume,
                Unit = t.Unit
            }).ToList();

            return Ok(taskDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto request)
        {
            var userId = GetUserId();

            // Find TaskType
            // Front-end sends 'compromisso', but in DB it's 'appointment'
            // We should map or just assume the frontend sends 'appointment', 'habit', 'deadline_goal'
            // The frontend modal sends activeTab, which is 'compromisso', 'habito', 'meta'.
            // Let's normalize it to the DB seeds: 'appointment', 'habit', 'deadline_goal'
            
            var typeName = request.Type.ToLower() switch
            {
                "compromisso" => "appointment",
                "habito" => "habit",
                "meta" => "deadline_goal",
                _ => request.Type.ToLower()
            };

            var taskType = await _context.TaskTypes.FirstOrDefaultAsync(t => t.Name == typeName);
            
            if (taskType == null)
            {
                return BadRequest($"TaskType '{typeName}' not found.");
            }

            var newTask = new TaskItem
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TaskTypeId = taskType.Id,
                Title = request.Name,
                Description = request.Description,
                Location = request.Location,
                Date = request.ScheduledFor.Date,
                Time = request.ScheduledFor.ToString("HH:mm"),
                Completed = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Task created successfully", taskId = newTask.Id });
        }

        [HttpPut("{id}/toggle")]
        public async Task<IActionResult> ToggleTask(Guid id, [FromBody] ToggleTaskDto request)
        {
            var userId = GetUserId();
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
            {
                return NotFound();
            }

            task.Completed = request.Completed;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }

    public class ToggleTaskDto
    {
        public bool Completed { get; set; }
    }
}
