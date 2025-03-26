using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FizzBuzz.Controllers;
using FizzBuzz.DTOs;
using FizzBuzz.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Newtonsoft.Json;


public class GameDefinitionControllerTests
{
    private readonly Mock<IGameDefinitionService> _mockService;
    private readonly GameDefinitionController _controller;

    public GameDefinitionControllerTests()
    {
        _mockService = new Mock<IGameDefinitionService>();
        _controller = new GameDefinitionController(_mockService.Object);
    }

    [Fact]
    public async Task CreateGame_Valid()
    {
        
        var request = new GameCreateRequestDTO 
        { 
            GameName = "test1", 
            Author = "abc",
            DivisorWordPairs = new List<DivisorWordPairDTO> { new DivisorWordPairDTO { Divisor = 1, Word = "yes" } }
        };
        var response = new FizzBuzzRule
        {
        Id = 1,
        GameName = "test1",
        Author = "abc" };
        
        _mockService.Setup(s => s.CreateGameAsync(It.IsAny<GameCreateRequestDTO>()))
            .ReturnsAsync(response);

        
        var result = await _controller.CreateGame(request);

        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        var json = JsonConvert.SerializeObject(okResult.Value);
        var returnedGame = JsonConvert.DeserializeObject<dynamic>(json);
        Assert.NotNull(returnedGame);
        Assert.Equal(response.GameName, (string)returnedGame.GameName);
        Assert.Equal(response.Author, (string)returnedGame.Author);
    }

    [Fact]
    public async Task CreateGame_Invalid()
    {
        _controller.ModelState.AddModelError("GameName", "Required");
        
        var result = await _controller.CreateGame(new GameCreateRequestDTO());
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task GetGameDefinitionByName_GameExists()
    {
        var gameName = "gametest";
        var gameDto = new FizzBuzzRuleDTO();
        _mockService.Setup(s => s.GetGameDefinitionByNameAsync(gameName)).ReturnsAsync(gameDto);

        var result = await _controller.GetGameDefinitionByName(gameName);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task GetGameDefinitionByName_GameDoesNotExist()
    {
        _mockService.Setup(s => s.GetGameDefinitionByNameAsync(It.IsAny<string>())).ReturnsAsync((FizzBuzzRuleDTO)null);

        var result = await _controller.GetGameDefinitionByName("NonExistentGame");
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetGameNames_GamesExist()
    {
        _mockService.Setup(s => s.GetGameNamesAsync()).ReturnsAsync(new List<string> { "game1" });

        var result = await _controller.GetGameNames();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task GetGameNames_NoGamesExist()
    {
        _mockService.Setup(s => s.GetGameNamesAsync()).ReturnsAsync(new List<string>());

        var result = await _controller.GetGameNames();
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetLastGameId_Valid()
    {
        _mockService.Setup(s => s.GetLastGameIdAsync()).ReturnsAsync(10);

        var result = await _controller.GetLastGameId();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(10, okResult.Value);
    }
    
}
