using Microsoft.EntityFrameworkCore;
using ScoreboardAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()    // Allow any domain
               .AllowAnyMethod()    // Allow any HTTP method
               .AllowAnyHeader();   // Allow any header
    });
});

// Configure the database context with SQL Server
builder.Services.AddDbContext<ScoreboardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Use CORS
app.UseCors();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();

    // Enable Swagger in all environments
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scoreboard API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger at the root URL
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
