using BusinessLogic.Services.GameSessionService;
using BusinessLogic.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenseCapitalTestTask.Requests;
using Shared.Entities;

namespace SenseCapitalTestTask.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GameSessionsController : ControllerBase
{
    private readonly IGameSessionService _gameSessionService;
    private readonly IUserService _userService;

    public GameSessionsController(IGameSessionService gameSessionService, IUserService userService)
    {
        _gameSessionService = gameSessionService;
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<GameSession>>> Get()
    {
        var response = await _gameSessionService.Get();

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GameSession>> Get([FromRoute] int id)
    {
        var response = await _gameSessionService.Get(id);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<GameSession>> CreateSession(CreateGameSessionRequest request)
    {
        var player = new Player
        {
            Side = request.PlayerSide,
            User = await GetAuthorizeUser()
        };

        var response = await _gameSessionService.CreateSession(player);
        
        return Ok(response);
    }

    [HttpPut("MakeMove")]
    public async Task<ActionResult<GameSession>> MakeMove(UpdateGameSessionRequest request)
    {
        var gameSession = await _gameSessionService.Get(request.SessionId);

        var user = await GetAuthorizeUser();

        if (gameSession.IsGameEnded)
        {
            return BadRequest("Игра завершена");
        }

        if (gameSession.PlayerTurn.UserId != user.Id)
        {
            return BadRequest($"Сейчас ход игрока #{gameSession.PlayerTurn.User.Username}");
        }

        try
        {
            var response = await _gameSessionService.MakeMove(gameSession, request.MatrixX, request.MatrixY, user.Id);

            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("JoinToSession/{id:int}")]
    public async Task<ActionResult<GameSession>> JoinToSession([FromRoute] int id)
    {
        var gameSession = await _gameSessionService.Get(id);
        
        var user = await GetAuthorizeUser();

        if (gameSession.FirstPlayer.UserId == user.Id || gameSession.SecondPlayer?.UserId == user.Id)
        {
            return BadRequest("Вы уже в сессии");
        }

        if (gameSession.SecondPlayer is not null)
        {
            return BadRequest("Сессия заполнена");
        }

        var response = await _gameSessionService.JoinToSession(id, user.Id);
        
        return Ok(response);
    }

    private async Task<User> GetAuthorizeUser()
    {
        var user = await _userService.GetByName(User.Identity.Name);

        return user;
    }
}