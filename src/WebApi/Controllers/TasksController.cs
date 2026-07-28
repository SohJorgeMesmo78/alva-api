using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
            try
            {
                var userId = GetUserId();

                var query = _context.Tasks
                    .Include(t => t.TaskType)
                    .Where(t => t.UserId == userId);

                if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out var parsedDate))
                {
                    var targetDate = parsedDate.Date;
                    var nextDay = targetDate.AddDays(1);

                    query = query.Where(t => 
                        (t.TaskType.Name == "appointment" && t.Date >= targetDate && t.Date < nextDay) ||
                        (t.TaskType.Name == "habit" && t.Date < nextDay) ||
                        (t.TaskType.Name == "deadline_goal" && t.Date < nextDay && (t.Deadline == null || t.Deadline >= targetDate))
                    );
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao buscar tarefas", error = ex.Message, inner = ex.InnerException?.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto request)
        {
            try
            {
                var userId = GetUserId();

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
                    return BadRequest($"TaskType '{typeName}' não encontrado.");
                }

                var scheduledFor = request.ScheduledFor.Date;
                var startDate = request.StartDate.HasValue 
                    ? request.StartDate.Value.Date
                    : scheduledFor;
                
                var deadline = request.Deadline.HasValue
                    ? request.Deadline.Value.Date.AddDays(1).AddSeconds(-1)
                    : (DateTime?)null;

                var newTask = new TaskItem
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    TaskTypeId = taskType.Id,
                    Title = request.Name,
                    Description = request.Description,
                    Location = request.Location,
                    Date = typeName switch
                    {
                        "appointment" => scheduledFor,
                        _ => startDate
                    },
                    Time = typeName == "appointment" ? request.ScheduledFor.ToString("HH:mm") : null,
                    Completed = false,
                    CreatedAt = DateTime.UtcNow,
                    Streak = typeName == "habit" ? 0 : null,
                    Deadline = deadline,
                    TargetVolume = request.TargetVolume,
                    CurrentVolume = 0,
                    Unit = request.Unit
                };

                _context.Tasks.Add(newTask);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Tarefa criada com sucesso", taskId = newTask.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao criar tarefa", error = ex.Message, inner = ex.InnerException?.Message });
            }
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

        [HttpPut("{id}/progress")]
        public async Task<IActionResult> UpdateProgress(Guid id, [FromBody] UpdateProgressDto request)
        {
            var userId = GetUserId();
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
            {
                return NotFound();
            }

            task.CurrentVolume = request.CurrentVolume;
            if (task.TargetVolume.HasValue && task.CurrentVolume >= task.TargetVolume)
            {
                task.Completed = true;
            }
            await _context.SaveChangesAsync();

            return Ok();
        }
    }

    public class ToggleTaskDto
    {
        public bool Completed { get; set; }
    }

    public class UpdateProgressDto
    {
        public int CurrentVolume { get; set; }
    }
}
