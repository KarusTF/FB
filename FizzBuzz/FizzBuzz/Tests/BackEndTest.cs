using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FizzBuzz.Controllers;
using FizzBuzz.Data;
using FizzBuzz.Models;

namespace FizzBuzz.Tests
{
    public class FizzBuzzGameControllerTests
    {
        private Mock<AppDbContext> _mockContext;
        private FizzBuzzGameController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<AppDbContext>();
            _controller = new FizzBuzzGameController(_mockContext.Object);
        }

        [Test]
        public IActionResult StartGame_ReturnsGameSession()
        {
            // Arrange
            var gameStartRequest = new GameStartRequest { TimeLimit = 10 };

            // Act
            var result = _controller.StartGame(gameStartRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            // Ensure the result is of type OkObjectResult
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            // Ensure the game session is correctly returned
            var gameSession = okResult.Value as GameSession;
            Assert.IsNotNull(gameSession);

            // Assert that the time limit matches
            Assert.AreEqual(gameStartRequest.TimeLimit, gameSession.TimeLimit);

            // Ensure that we are returning a valid result
            return result;  // Make sure all code paths return a value
        }
    }
}