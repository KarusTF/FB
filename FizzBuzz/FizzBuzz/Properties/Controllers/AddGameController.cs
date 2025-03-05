using Microsoft.AspNetCore.Mvc;
using FizzBuzz.Data;
using FizzBuzz.Models; 
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FizzBuzz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    
    public class FizzBuzzRulesController : ControllerBase
    {
        public async Task<int> GetLastGameId()
        {
            // Fetch the last game based on the highest Id
            var lastGame = await _context.FizzBuzzRules
                .OrderByDescending(game => game.Id)
                .FirstOrDefaultAsync();

            if (lastGame != null)
            {
                return lastGame.Id; 
            }

            return 0; 
        }
        private readonly AppDbContext _context;

        public FizzBuzzRulesController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/fizzbuzzrules
        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] GameCreationRequest request)
        {
            /*
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }
            // Validate input
            if (request.DPs == null || request.DPs.Count <1)
            {
                return BadRequest("You must provide at least 1 divisor-word pair.");
            }
            */
            // Create new FizzBuzzRule and associated DivisorWordPairs
            var game = new FizzBuzzRule
            {
                GameName = request.GameName,
                Author = request.Author
            };

            foreach (var pair in request.DPs)
            {
                if (pair.Divisor <= 0)
                {
                    return BadRequest("Divisors must be positive numbers.");
                }
            }

            _context.FizzBuzzRules.Add(game);
            await _context.SaveChangesAsync(); // Save the game first to get the auto-generated ID

            var newPairs = new List<DivisorWordPair>();
            
            Console.WriteLine(request.Author);

            foreach (var pair in request.DPs)
            {
                var newPair = new DivisorWordPair
                {
                    Divisor = pair.Divisor,
                    Word = pair.Word,
                    FizzBuzzRuleId = game.Id 
                };
                Console.WriteLine($"New Pair - Divisor: {newPair.Divisor}, Word: {newPair.Word}, GameId: {newPair.FizzBuzzRuleId}");
                newPairs.Add(newPair); 
            }
            _context.DivisorWordPairs.AddRange(newPairs);
            await _context.SaveChangesAsync();

            return Ok(new { game.Id, game.GameName, game.Author });
        }
    }

    public class GameCreationRequest
    {
        public string GameName { get; set; }
        public string Author { get; set; }
        public List<DP> DPs { get; set; }
    }

    
    public class DP
    {
        public int Divisor { get; set; }
        public string Word { get; set; }

        // Constructor
        public DP(int divisor, string word)
        {
            Divisor = divisor;
            Word = word;
        }
    }
}
