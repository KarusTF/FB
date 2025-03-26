using FizzBuzz.Data;
using FizzBuzz.DTOs; // Import the DTOs namespace
using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FizzBuzz.Services
{
    public class GameDefinitionService : IGameDefinitionService
    {
        private readonly AppDbContext _context;

        public GameDefinitionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FizzBuzzRule> CreateGameAsync(GameCreateRequestDTO request)
        {
            // Validate input
            if (request == null)
            {
                throw new ArgumentException("Request cannot be null.");
            }

            if (request.DivisorWordPairs == null || request.DivisorWordPairs.Count < 1)
            {
                throw new ArgumentException("At least one divisor-word pair is required.");
            }
            

            
            var lastGame = await _context.FizzBuzzRules
                .OrderByDescending(f => f.Id)  
                .FirstOrDefaultAsync(); 
            
            var lastGameId = lastGame?.Id ?? 0; //handle when no game in database
            var CurrentGameId = lastGameId + 1; 
            var game = new FizzBuzzRule
                
            {
                GameName = request.GameName,
                Author = request.Author,
                Id = CurrentGameId,
                
            };
            if (game.DivisorWordPairs == null)
            {
                game.DivisorWordPairs = new List<DivisorWordPair>();
            }
            
            

            foreach (var pairDto in request.DivisorWordPairs)
            {
                if (pairDto.Divisor <= 0)
                {
                    throw new ArgumentException("Divisors must be positive numbers.");
                }

                
                var divisorWordPair = new DivisorWordPair
                {
                    FizzBuzzRuleId = CurrentGameId, 
                    Divisor = pairDto.Divisor,      
                    Word = pairDto.Word           
                    
                };

                
                game.DivisorWordPairs.Add(divisorWordPair);
            }

            
            _context.FizzBuzzRules.Add(game);
            
            await _context.SaveChangesAsync(); 

            return game;
        }

        public async Task<FizzBuzzRuleDTO> GetGameDefinitionByNameAsync(string gameName)
        {
            var game = await _context.FizzBuzzRules
                .Include(f => f.DivisorWordPairs)
                .FirstOrDefaultAsync(f => f.GameName == gameName);
            
            

            if (game == null)
                return null;

            return new FizzBuzzRuleDTO
            {
                GameName = game.GameName,
                Author = game.Author,
                DivisorWordPairs = game.DivisorWordPairs.Select(pair => new DivisorWordPairDTO
                {
                    Divisor = pair.Divisor,
                    Word = pair.Word,
                }).ToList()
            };
        }

        public async Task<List<string>> GetGameNamesAsync()
        {
            return await _context.FizzBuzzRules
                .Select(rule => rule.GameName)
                .ToListAsync();
        }

        public async Task<int> GetLastGameIdAsync()
        {
            var lastGame = await _context.FizzBuzzRules
                .OrderByDescending(game => game.Id)
                .FirstOrDefaultAsync();

            return lastGame?.Id ?? 0; // Return 0 if no game
        }
    }
}