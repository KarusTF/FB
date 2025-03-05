using System.Text.Json.Serialization;
using FizzBuzz.Data;
using Microsoft.EntityFrameworkCore;
using FizzBuzz.Models;
using FizzBuzz.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configure SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins("http://localhost:3000")  // Frontend and Backend different hosts
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.MaxDepth = 32;
});

var app = builder.Build();

// The below code is currently commented out but it is part of my initialization logic
/*
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    if (!context.FizzBuzzRules.Any())
    {
        var rule1 = new FizzBuzzRule 
        { 
            GameName = "Game1", 
            Author = "Admin2", 
            DivisorWordPairs = new List<DivisorWordPair>
            {
                new DivisorWordPair { Divisor = 3, Word = "Fizz" },
                new DivisorWordPair { Divisor = 5, Word = "Buzz" }
            }
        };
        
        var rule2 = new FizzBuzzRule 
        { 
            GameName = "Game2", 
            Author = "Admin1", 
            DivisorWordPairs = new List<DivisorWordPair>
            {
                new DivisorWordPair { Divisor = 3, Word = "Foo" },
                new DivisorWordPair { Divisor = 7, Word = "Bar" }
            }
        };

        var rule3 = new FizzBuzzRule 
        { 
            GameName = "Game3", 
            Author = "Admin", 
            DivisorWordPairs = new List<DivisorWordPair>
            {
                new DivisorWordPair { Divisor = 4, Word = "Qux" },
                new DivisorWordPair { Divisor = 6, Word = "Quux" }
            }
        };
        
        context.FizzBuzzRules.AddRange(rule1, rule2, rule3);
        context.SaveChanges();
    }
}
*/

app.UseCors("AllowReactApp");
// Configure middleware
app.UseRouting();
app.MapControllers();

app.Run();
