using SiteConstructor.Data;
using SiteConstructor.Services;
using Microsoft.EntityFrameworkCore;
using SiteConstructor.Models;
using System.Text.Json;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ContentService>();
builder.Services.AddScoped<IDataStore, PostgresDataStore>();
builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var store = scope.ServiceProvider.GetRequiredService<IDataStore>();
    if (!await db.TeamMembers.AnyAsync())
    {
        if (File.Exists("data.json"))
        {
            var json = await File.ReadAllTextAsync("data.json");
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            var content = JsonSerializer.Deserialize<Content>(json, options)!;
            await store.WriteAsync(content);
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Use(async (context, next) =>
{
    try
    {
        await next(); 
    }
    catch (ArgumentException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";        
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseDefaultFiles();

app.UseStaticFiles();

app.MapControllers();

app.Run();

