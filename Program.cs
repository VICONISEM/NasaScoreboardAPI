using Microsoft.EntityFrameworkCore;
using ScoreboardAPI.Data;
using ScoreboardAPI.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddSignalR(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()    
               .AllowAnyMethod()    
               .AllowAnyHeader();   
    });
});


builder.Services.AddDbContext<ScoreboardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped<ITeamOperation, TeamOperations>();

var app = builder.Build();


app.UseCors();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();

    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scoreboard API v1");
        c.RoutePrefix = string.Empty; 
    });
}
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.MapHub<ScoreboardHub>("/scoreboardHub").AllowAnonymous();

app.Run();
