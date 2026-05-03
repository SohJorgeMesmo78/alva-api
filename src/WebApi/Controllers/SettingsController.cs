using System.Security.Claims;
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
    public class SettingsController : ControllerBase
    {
        private readonly AlvaDbContext _context;

        public SettingsController(AlvaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var settings = await _context.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId);
            if (settings == null) return NotFound();

            return Ok(UserSettingsDto.FromEntity(settings));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSettings([FromBody] UserSettingsDto request)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var settings = await _context.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId);
            if (settings == null) return NotFound();

            settings.Language = request.Language;
            settings.Country = request.Country;
            settings.Timezone = request.Timezone;
            settings.DateFormat = request.DateFormat;
            settings.TimeFormat = request.TimeFormat;
            settings.Theme = request.Theme;
            settings.PrimaryColor = request.PrimaryColor;
            settings.StartOfWeek = request.StartOfWeek;
            settings.NotificationsEnabled = request.NotificationsEnabled;
            settings.SoundEnabled = request.SoundEnabled;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Settings updated successfully" });
        }
    }
}
