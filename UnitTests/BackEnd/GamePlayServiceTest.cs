using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FizzBuzz.Data;
using FizzBuzz.Models;
using FizzBuzz.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FizzBuzz.Tests
{
    public class GamePlayServiceTests : IDisposable
    {
        private readonly GamePlayService _gamePlayService;
        private readonly AppDbContext _context;
        private readonly SqliteConnection _connection;

        public GamePlayServiceTests()
        {
            // Initialise database and set up for test file
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new AppDbContext(options);
            
            _context.Database.EnsureCreated();
            SeedTestData();

            _gamePlayService = new GamePlayService(_context);
        }

        private void SeedTestData()
        {
            
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            

            
            _context.FizzBuzzRules.Add(new FizzBuzzRule
            {
                GameName = "TestGame",
                Author = "TestAuthor",
                DivisorWordPairs = new List<DivisorWordPair>
                {
                    new DivisorWordPair { Divisor = 2, Word = "yes" },
                    new DivisorWordPair { Divisor = 9, Word = "no" }
                }
            });
            _context.SaveChanges();

        }

        public void Dispose()
        {
            _connection?.Dispose();
            _context?.Dispose();
        }

        [Fact]
        public async Task StartGameAsync_ValidGame()
        {
            
            int timeLimit = 5;

            
            var gameSession = await _gamePlayService.StartGameAsync(timeLimit);

            
            Assert.NotNull(gameSession);
            Assert.Equal(0, gameSession.CorrectAnswers);
            Assert.Equal(0, gameSession.IncorrectAnswers);
            Assert.NotNull(gameSession.GameId);
        }

        [Fact]
        public async Task SubmitAnswerAsync_CorrectResult()
        {
            
            var gameSession = await _gamePlayService.StartGameAsync(5);
            var result = await _gamePlayService.SubmitAnswerAsync("TestGame", 2,"yes");

            
            Assert.NotNull(result);
            Assert.True(result.IsCorrect);
        }

        [Fact]
        public async Task SubmitAnswerAsync_IncorrectResult()
        {

            int number = 4;
            string answer = "IncorrectAnswer";

            
            var result = await _gamePlayService.SubmitAnswerAsync("TestGame", number, answer);

            
            Assert.NotNull(result);
            Assert.False(result.IsCorrect);
        }
        [Fact]
        public async Task SubmitAnswerAsync_GameNotFound()
        {
            
            string NotFoundGame = "nogame"; 
            int number = 15;
            string answer = "FizzBuzz";

            
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _gamePlayService.SubmitAnswerAsync(NotFoundGame, number, answer));

            Assert.Equal("Game rule not found.", exception.Message);
        }
        
    }
}