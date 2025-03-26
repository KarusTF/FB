using FizzBuzz.Controllers;
using FizzBuzz.DTOs;
using FizzBuzz.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

namespace FizzBuzz.UnitTests.BackEnd
{
    public class GamePlayControllerTest
    {
        private readonly GamePlayController _controller;
        private readonly Mock<IGamePlayService> _mockGamePlayService;

        public GamePlayControllerTest()
        {
            _mockGamePlayService = new Mock<IGamePlayService>();
            _controller = new GamePlayController(_mockGamePlayService.Object);
        }
        
        [Fact]
        public async Task StartGame_Valid()
        {
            var request = new GameStartRequestDTO { TimeLimit = 10 };
            var gameSession = new GameSessionDTO { 
                GameId = Guid.NewGuid().ToString(),
                StartTime = DateTime.UtcNow,
                CorrectAnswers = 0,
                IncorrectAnswers = 0, 
            };  

            _mockGamePlayService.Setup(s => s.StartGameAsync(request.TimeLimit))
                .ReturnsAsync(gameSession);

            
            var result = await _controller.StartGame(request);

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGameSession = Assert.IsType<GameSessionDTO>(okResult.Value);
            Assert.NotNull(returnedGameSession);
            Assert.Equal(0, returnedGameSession.CorrectAnswers);
            Assert.Equal(0, returnedGameSession.IncorrectAnswers);
        }

        [Fact]
        public async Task StartGame_Invalid()
        {
            
            var request = new GameStartRequestDTO { TimeLimit = -1 };  // Invalid TimeLimit

            _controller.ModelState.AddModelError("TimeLimit", "TimeLimit cannot be negative");
            
            var result = await _controller.StartGame(request);

            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, errorResponse.ErrorCode);
        }

        [Fact]
        public async Task StartGame_Invalid_Time()
        {
            var request = new GameStartRequestDTO { TimeLimit = 10 };

            _mockGamePlayService.Setup(s => s.StartGameAsync(request.TimeLimit))
                .ThrowsAsync(new ArgumentException("Invalid time limit"));

            
            var result = await _controller.StartGame(request);

            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("Invalid time limit", errorResponse.Message);
        }

        [Fact]
        public async Task StartGame_InternalServerError()
        {
            
            var request = new GameStartRequestDTO { TimeLimit = 10 };

            _mockGamePlayService.Setup(s => s.StartGameAsync(request.TimeLimit))
                .ThrowsAsync(new Exception("Unexpected error"));

            
            var result = await _controller.StartGame(request);

            
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
        }
        

        [Fact]
        public async Task SubmitAnswer_Valid()
        {
            
            var gameId = "1";
            var submission = new AnswerSubmissionDTO { Number = 9, Answer = "yes" };
            var result = new AnswerResultDTO { IsCorrect = true };  

            _mockGamePlayService.Setup(s => s.SubmitAnswerAsync(gameId, submission.Number, submission.Answer))
                .ReturnsAsync(result);

            
            var actionResult = await _controller.SubmitAnswer(gameId, submission.Number, submission);

            
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnedResult = Assert.IsType<AnswerResultDTO>(okResult.Value);
            Assert.Equal(true, returnedResult.IsCorrect);
        }

        [Fact]
        public async Task SubmitAnswer_Invalid_Number()
        {
            
            var gameId = "2";
            var submission = new AnswerSubmissionDTO { Number = -1, Answer = "yes" };  

            _controller.ModelState.AddModelError("Number", "Number must be positive");

            
            var result = await _controller.SubmitAnswer(gameId, submission.Number, submission);

            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, errorResponse.ErrorCode);
        }

        [Fact]
        public async Task SubmitAnswer_Invalid_Answer()
        {
            
            var gameId = "2";
            var submission = new AnswerSubmissionDTO { Number = 1, Answer = "yes" };

            _mockGamePlayService.Setup(s => s.SubmitAnswerAsync(gameId, submission.Number, submission.Answer))
                .ThrowsAsync(new ArgumentException("Invalid answer"));

            
            var result = await _controller.SubmitAnswer(gameId, submission.Number, submission);

            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("Invalid answer", errorResponse.Message);
        }

        [Fact]
        public async Task SubmitAnswer_Internal_Invalid()
        {
            
            var gameId = "3";
            var submission = new AnswerSubmissionDTO { Number = 1, Answer = "yes" };

            _mockGamePlayService.Setup(s => s.SubmitAnswerAsync(gameId, submission.Number, submission.Answer))
                .ThrowsAsync(new Exception("Unexpected error"));

            
            var result = await _controller.SubmitAnswer(gameId, submission.Number, submission);

            
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
        }
        
        
    }
}
