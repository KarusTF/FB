using Microsoft.AspNetCore.Mvc;
using FizzBuzz.Data;
using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class FizzBuzzRulesController : ControllerBase
{
    private readonly AppDbContext _context;

    public FizzBuzzRulesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetRules()
    {
        var rules = _context.FizzBuzzRules.Include(rule => rule.DivisorWordPairs).ToList();
        return Ok(rules);
    }

    

    [HttpGet("{gameName}")]
    public async Task<ActionResult<FizzBuzzRule>> GetFizzBuzzRuleByGameName(string gameName)
    {
        
        var game = await _context.FizzBuzzRules
            .Include(f => f.DivisorWordPairs) 
            .FirstOrDefaultAsync(f => f.GameName == gameName);  

        

        return Ok(game);  
    }
    
    [HttpGet("games")]
    public async Task<ActionResult<List<string>>> GetGameNames()
    {
        try
        {
            // Fetch game names from FizzBuzzRule table
            var gameNames = await _context.FizzBuzzRules
                .Select(fbr => fbr.GameName)  // Select only the GameName
                .ToListAsync();  // Convert to list asynchronously

            if (gameNames == null || gameNames.Count == 0)
            {
                return NotFound("No game available");
            }

            return Ok(gameNames);  // Return the list of game names
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}