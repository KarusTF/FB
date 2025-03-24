using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FizzBuzz.DTOs;
using FizzBuzz.Services;

namespace FizzBuzz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameDefinitionController : ControllerBase
    {
        private readonly IGameDefinitionService _service;

        public GameDefinitionController(IGameDefinitionService service)
        {
            _service = service;
        }

        // POST: api/gamedefinition
        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] GameCreateRequestDTO request)
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
                var game = await _service.CreateGameAsync(request);
                return Ok(new { game.Id, game.GameName, game.Author });
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

        // GET: api/gamedefinition/{gameName}
        [HttpGet("{gameName}")]
        public async Task<ActionResult<FizzBuzzRuleDTO>> GetGameDefinitionByName(string gameName)
        {
            try
            {
                var gameDTO = await _service.GetGameDefinitionByNameAsync(gameName);
                if (gameDTO == null)
                {
                    return NotFound(new ApiErrorResponse
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        Message = "Game not found.",
                        Details = "No game found with the provided name."
                    });
                }

                return Ok(gameDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiErrorResponse
                {
                    ErrorCode = StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error occurred.",
                    Details = ex.Message
                });
            }
        }

        // GET: api/gamedefinition/games
        [HttpGet("games")]
        public async Task<ActionResult<List<string>>> GetGameNames()
        {
            try
            {
                var gameNames = await _service.GetGameNamesAsync();
                if (gameNames.Count == 0)
                {
                    return NotFound(new ApiErrorResponse
                    {
                        ErrorCode = StatusCodes.Status404NotFound,
                        Message = "No games available.",
                        Details = "Game database is empty."
                    });
                }

                return Ok(gameNames);
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

        // GET: api/gamedefinition/last-game-id
        [HttpGet("last-game-id")]
        public async Task<ActionResult<int>> GetLastGameId()
        {
            try
            {
                var lastGameId = await _service.GetLastGameIdAsync();
                return Ok(lastGameId);
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