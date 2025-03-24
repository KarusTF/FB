using Microsoft.EntityFrameworkCore;
using FizzBuzz.Data;
using FizzBuzz.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IGameDefinitionService, GameDefinitionService>();
builder.Services.AddScoped<IGamePlayService, GamePlayService>();

builder.Services.AddScoped<IGamePlayService, GamePlayService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:5155") // Allow frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build(); 

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowLocalhost");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();