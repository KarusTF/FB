using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FizzBuzz.DTOs;
using FizzBuzz.Services;

namespace FizzBuzz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamePlayController : ControllerBase
    {
        private readonly IGamePlayService _gamePlayService;

        public GamePlayController(IGamePlayService gamePlayService)
        {
            _gamePlayService = gamePlayService;
        }

        // POST: api/gameplay/start
        [HttpPost("start")]
        public async Task<IActionResult> StartGame([FromBody] GameStartRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiErrorResponse
                {
                    ErrorCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid input.",
                    Details = ModelState.ToString()
                });
            }

            try
            {
                var gameSession = await _gamePlayService.StartGameAsync(request.TimeLimit);
                return Ok(gameSession);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiErrorResponse
                {
                    ErrorCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    Details = "Invalid input provided."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiErrorResponse
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error.",
                    Details = ex.Message
                });
            }
        }

        // POST: api/gameplay/submit/{gameId}
        [HttpPost("submit/{gameId}")]
        public async Task<IActionResult> SubmitAnswer(string gameId, int number, [FromBody] AnswerSubmissionDTO submission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiErrorResponse
                {
                    ErrorCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid input.",
                    Details = ModelState.ToString()
                });
            }

            try
            {
                var result = await _gamePlayService.SubmitAnswerAsync(gameId, submission.Number, submission.Answer);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiErrorResponse
                {
                    ErrorCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    Details = "Invalid input."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiErrorResponse
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error.",
                    Details = ex.Message
                });
            }
        }
        
    }
}