using Microsoft.AspNetCore.Mvc;
using FizzBuzz.Data;
using Microsoft.EntityFrameworkCore; 
using System;
using System.Collections.Generic;
using System.Linq;


namespace FizzBuzz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FizzBuzzGameController : ControllerBase
    {
        private static Random _random = new Random();
        private static HashSet<int> _usedNumbers = new HashSet<int>(); 
        private readonly AppDbContext _context; 

        public FizzBuzzGameController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/fizzbuzzgame/start
        [HttpPost("start")]
        public IActionResult StartGame([FromBody] GameStartRequest request)
        {
            var gameRule = _context.FizzBuzzRules.FirstOrDefault();
            
            var gameSession = new GameSession
            {
                GameId = Guid.NewGuid().ToString(),
                TimeLimit = request.TimeLimit,
                StartTime = DateTime.UtcNow,
                CorrectAnswers = 0,
                IncorrectAnswers = 0,
                UsedNumbers = new HashSet<int>(_usedNumbers)
            };

            return Ok(gameSession);
        }

        // GET: api/fizzbuzzgame/next/{gameId}
        [HttpGet("next/{gameId}")]
        public IActionResult GetNextNumber(string gameId)
        {
           
            int randomNumber;
            do
            {
                randomNumber = _random.Next(1, 100);  
            } while (_usedNumbers.Contains(randomNumber));

            _usedNumbers.Add(randomNumber);

            return Ok(new { randomNumber });
        }
        
        

        // POST: api/fizzbuzzgame/submit/{gameId}
        [HttpPost("submit/{gameId}")]
        public IActionResult SubmitAnswer(string gameId, [FromBody] AnswerSubmission submission)
        {
           
            var gameRule = _context.FizzBuzzRules
                .Include(rule => rule.DivisorWordPairs) 
                .FirstOrDefault(rule => rule.GameName == gameId);

            
            bool isCorrect = ValidateAnswer(submission.Number, submission.Answer, gameRule.DivisorWordPairs);

            if (isCorrect)
            {
                submission.CorrectAnswers++;
            }
            else
            {
                submission.IncorrectAnswers++;
            }

            return Ok(new
            {
                isCorrect,
                correctAnswers = submission.CorrectAnswers,
                incorrectAnswers = submission.IncorrectAnswers
            });
        }
        
        
        private bool ValidateAnswer(int number, string answer, List<DivisorWordPair> divisorWordPairs)
        {
            // Construct the correct answer
            string correctAnswer = "";

            foreach (var pair in divisorWordPairs)
            {
                if (number % pair.Divisor == 0)
                {
                    correctAnswer += pair.Word;
                }
                
            }


            if (string.IsNullOrEmpty(correctAnswer))
            {
                correctAnswer = number.ToString();
            }

            // case-insensitive answer compared
            return correctAnswer.Equals(answer, StringComparison.OrdinalIgnoreCase);
        }
    }


    public class GameStartRequest
    {
        
        public int TimeLimit { get; set; }  
    }

    public class AnswerSubmission
    {
        public int Number { get; set; }
        public string Answer { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }
    }

    public class GameSession
    {
        public string GameId { get; set; }
        public int TimeLimit { get; set; }
        public DateTime StartTime { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }
        public HashSet<int> UsedNumbers { get; set; }
    }
}
