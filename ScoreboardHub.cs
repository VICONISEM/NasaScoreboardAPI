using Microsoft.AspNetCore.SignalR;
using ScoreboardAPI.Models;

public class ScoreboardHub : Hub
{
    public async Task SendScoreUpdate(Team team)
    {
        await Clients.All.SendAsync("ReceiveScoreUpdate",team);
    }
}
