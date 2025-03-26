using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzBuzz.Data;
using FizzBuzz.DTOs;
using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzz.Services
{
    public class GamePlayService : IGamePlayService
    {
        private static Random _random = new Random();
        private readonly AppDbContext _context;

        public GamePlayService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GameSessionDTO> StartGameAsync(int timeLimit)
        {
            var gameRule = await _context.FizzBuzzRules.FirstOrDefaultAsync();

            var gameSession = new GameSessionDTO
            {
                GameId = Guid.NewGuid().ToString(),
                StartTime = DateTime.UtcNow,
                CorrectAnswers = 0,
                IncorrectAnswers = 0,
            };

            return gameSession;
        }
        
        public async Task<AnswerResultDTO> SubmitAnswerAsync(string gameNameInput, int number, string answer)
        {
            var gameRule = await _context.FizzBuzzRules
                .Include(rule => rule.DivisorWordPairs)
                .FirstOrDefaultAsync(rule => rule.GameName == gameNameInput);

            if (gameRule == null)
            {
                throw new ArgumentException("Game rule not found.");
            }

            bool isCorrect = ValidateAnswer(number, answer, gameRule.DivisorWordPairs);

            var result = new AnswerResultDTO
            {
                IsCorrect = isCorrect
            };

            return result;
        }

        private bool ValidateAnswer(int number, string answer, List<DivisorWordPair> divisorWordPairs)
        {
            string correctAnswer = "";
            string correctAnswer1 = "";

            int count = 0;

            foreach (var pair in divisorWordPairs)
            {
                if (number % pair.Divisor == 0)
                {
                    if (count == 0)
                    {
                        correctAnswer = pair.Word;
                        count++;
                    }
                    else
                    {
                        correctAnswer1 = pair.Word;
                    }
                }
            }

            if (string.IsNullOrEmpty(correctAnswer))
            {
                correctAnswer = number.ToString();
            }

            return correctAnswer.Equals(answer, StringComparison.OrdinalIgnoreCase) || correctAnswer1.Equals(answer, StringComparison.OrdinalIgnoreCase);
        }
    }
}