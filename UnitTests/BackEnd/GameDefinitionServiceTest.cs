using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzBuzz.Data;
using FizzBuzz.DTOs;
using FizzBuzz.Models;
using FizzBuzz.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace FizzBuzz.Tests
{
    public class GameDefinitionServiceTests : IDisposable
    {
        private readonly GameDefinitionService _service;
        private readonly AppDbContext _context;
        private readonly SqliteConnection _connection;
        private readonly ITestOutputHelper _output;

        public GameDefinitionServiceTests(ITestOutputHelper output)
        {
            //initialise memory database and set up for tests
            _output = output;
            
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new AppDbContext(options);
            
            _context.Database.EnsureCreated();

            _service = new GameDefinitionService(_context);
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _context?.Dispose();
        }

        [Fact]
        public async Task CreateGameAsync_Valid()
        {
            var request = new GameCreateRequestDTO
            {
                GameName = "TestGameCreate",
                Author = "TestAuthor",
                DivisorWordPairs = new List<DivisorWordPairDTO>
                {
                    new DivisorWordPairDTO { Divisor = 2, Word = "yes" },
                    new DivisorWordPairDTO { Divisor = 9, Word = "no" }
                }
            };

            var result = await _service.CreateGameAsync(request);

            Assert.NotNull(result);
            Assert.Equal("TestGameCreate", result.GameName);
            Assert.Equal(2, result.DivisorWordPairs.Count);
        }

        [Fact]
        public async Task CreateGameAsync_AfterExistenceGames()
        {
            _context.FizzBuzzRules.Add(new FizzBuzzRule
            {
                GameName = "Game1",
                Author = "Game2",
                DivisorWordPairs = new List<DivisorWordPair>
                {
                    new DivisorWordPair { Divisor = 4, Word = "four" }
                }
            });
            await _context.SaveChangesAsync();

            var request = new GameCreateRequestDTO
            {
                GameName = "AddNewGameTest",
                Author = "AddAuthorTest",
                DivisorWordPairs = new List<DivisorWordPairDTO>
                {
                    new DivisorWordPairDTO { Divisor = 9, Word = "chin" }
                }
            };

            var result = await _service.CreateGameAsync(request);

            Assert.Equal("AddNewGameTest", result.GameName); 
            Assert.Equal("AddAuthorTest", result.Author); 
        }

        [Fact]
        public async Task CreateGameAsync_Invalid()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.CreateGameAsync(null));

            // empty divisor - word pair list
            var emptyRequest = new GameCreateRequestDTO
            {
                GameName = "InvalidGame",
                Author = "InvalidAuthor",
                DivisorWordPairs = new List<DivisorWordPairDTO>()
            };
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.CreateGameAsync(emptyRequest));

            // negative number
            var invalidRequest = new GameCreateRequestDTO
            {
                GameName = "InvalidGame1",
                Author = "TestAuthor1",
                DivisorWordPairs = new List<DivisorWordPairDTO>
                {
                    new DivisorWordPairDTO { Divisor = -1, Word = "Zero" }
                }
            };
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.CreateGameAsync(invalidRequest));
        }

        [Fact]
        public async Task GetGameDefinitionByNameAsync_ReturnGames()
        {
            
            _context.FizzBuzzRules.Add(new FizzBuzzRule
            {
                GameName = "GetGame",
                Author = "Author",
                DivisorWordPairs = new List<DivisorWordPair>
                {
                    new DivisorWordPair { Divisor = 1, Word = "now" }
                }
            });
            await _context.SaveChangesAsync();

            
            var result = await _service.GetGameDefinitionByNameAsync("GetGame");

            
            Assert.NotNull(result);
            Assert.Equal("GetGame", result.GameName);
            Assert.Single(result.DivisorWordPairs);
        }

        [Fact]
        public async Task GetGameDefinitionByNameAsync_ReturnsNull()
        {
            
            var result = await _service.GetGameDefinitionByNameAsync("NonExistent");

            
            Assert.Null(result);
        }

        [Fact]
        public async Task GetGameNamesAsync()
        {
            
            _context.FizzBuzzRules.AddRange(
                new FizzBuzzRule { GameName = "Game1", Author = "1", 
                    DivisorWordPairs = new List<DivisorWordPair> {new DivisorWordPair { Divisor = 12, Word = "none" }}},
                new FizzBuzzRule { GameName = "Game2", Author = "2", 
                    DivisorWordPairs = new List<DivisorWordPair> {new DivisorWordPair { Divisor = 13, Word = "none" }}});
            await _context.SaveChangesAsync();

            
            var result = await _service.GetGameNamesAsync();

            
            Assert.Contains("Game1", result);
            Assert.Contains("Game2", result);
        }
        
    }
}