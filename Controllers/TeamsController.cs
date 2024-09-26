using Microsoft.AspNetCore.Mvc;
using ScoreboardAPI.Models;
using ScoreboardAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ScoreboardContext _context;
        private readonly IHubContext<ScoreboardHub> _hubContext;

        public TeamsController(ScoreboardContext context, IHubContext<ScoreboardHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            var teams = await _context.Teams.ToListAsync();

            foreach (var team in teams)
            {
                if (!string.IsNullOrEmpty(team.PhotoPath))
                {
                   
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", team.PhotoPath.TrimStart('/'));

                    
                    if (System.IO.File.Exists(filePath))
                    {
                        var imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                        team.PhotoBase64 = Convert.ToBase64String(imageBytes);
                    }
                }
            }

            return Ok(teams);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

           
            if (!string.IsNullOrEmpty(team.PhotoPath))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", team.PhotoPath.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    var imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    team.PhotoBase64 = Convert.ToBase64String(imageBytes);
                }
            }

            return Ok(team);
        }



        [HttpPost]
        public async Task<ActionResult<Team>> AddTeam([FromForm] Team newTeam, IFormFile photo)
        {
            if (newTeam == null || _context.Teams.Any(t => t.Id == newTeam.Id))
            {
                return BadRequest("Invalid team data");
            }

          
            if (photo != null && photo.Length > 0)
            {
               
                var fileName = $"{newTeam.Name}_{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                newTeam.PhotoPath = $"/Images/{fileName}";

                
                var imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                newTeam.PhotoBase64 = Convert.ToBase64String(imageBytes);
            }

            _context.Teams.Add(newTeam);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeam), new { id = newTeam.Id }, newTeam);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTeamScore(int id, Team updatedTeam)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            team.Score = updatedTeam.Score;
            await _context.SaveChangesAsync();

            
            await _hubContext.Clients.All.SendAsync("ReceiveScoreUpdate", team);

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            
            await _hubContext.Clients.All.SendAsync("ReceiveTeamDeletion", id);

            return NoContent();
        }
    }
}
