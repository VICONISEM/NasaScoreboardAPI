using Microsoft.AspNetCore.Mvc;
using ScoreboardAPI.Models;
using ScoreboardAPI.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ScoreboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ScoreboardContext _context;

        public TeamsController(ScoreboardContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            return await _context.Teams.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }

        
        [HttpPost]
        public async Task<ActionResult<Team>> AddTeam(Team newTeam)
        {
            if (newTeam == null || _context.Teams.Any(t => t.Id == newTeam.Id))
            {
                return BadRequest("Invalid team data");
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
            return NoContent();
        }
    }
}
