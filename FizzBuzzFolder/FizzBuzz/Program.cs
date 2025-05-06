using Microsoft.EntityFrameworkCore;
using FizzBuzz.Data;
using FizzBuzz.Services;

//to start the project => dotnet build => dotnet run
//navigate to fizzbuzz-frontend => npm run dev
//test frontend: navigate to UnitTests/FrontEnd => npm test
//test_ backend: navigate to UnitTests => dotnet test

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);  // HTTP port
    
});

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
            policy.AllowAnyOrigin() 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build(); 

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Applies pending migrations
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseCors("AllowLocalhost");
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles(); 
app.MapFallbackToFile("index.html");
app.Run();

