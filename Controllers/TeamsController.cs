using Microsoft.AspNetCore.Mvc;
using ScoreboardAPI.Models;
using ScoreboardAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreboardAPI.Handler;

namespace ScoreboardAPI.Controllers
{
    [Route("api/[controller]/[action]")]
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
                   
                    TeamOperations.UrlPhoto(Request, team);

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
                TeamOperations.UrlPhoto(Request, team);
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

                

                TeamOperations.FileHandlerAddPhoto(newTeam, photo);

                
           
            }
                 TeamOperations.Sum(newTeam);
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


             TeamOperations.Clone(team, updatedTeam);
           
   
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
            if (!string.IsNullOrEmpty(team.PhotoPath))
            {
              
                TeamOperations.FileHandlerDeletePhoto(team);
            }
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            
            await _hubContext.Clients.All.SendAsync("ReceiveTeamDeletion", id);

            return NoContent();
        }
    }
}
