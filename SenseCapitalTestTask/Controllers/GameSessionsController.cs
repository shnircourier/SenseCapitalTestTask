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

        if (response is null)
        {
            return NotFound("Сессия не найдена");
        }

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<GameSession>> CreateSession(CreateGameSessionRequest request)
    {
        var gameSession = new GameSession
        {
            FirstPlayerSide = request.PlayerSide,
            FirstPlayerId = (await GetAuthorizeUser()).Id
        };

        var response = await _gameSessionService.CreateSession(gameSession);
        
        return Ok(response);
    }

    [HttpPut("MakeMove")]
    public async Task<ActionResult<GameSession>> MakeMove(UpdateGameSessionRequest request)
    {
        var gameSession = await _gameSessionService.Get(request.SessionId);

        if (gameSession.SecondPlayerId == null)
        {
            return BadRequest("Вы не можете походить пока отсутсвует 2-й игрок");
        }

        var user = await GetAuthorizeUser();

        if (gameSession.IsGameEnded)
        {
            return BadRequest("Игра завершена");
        }

        if (gameSession.PlayerTurnId != user.Id)
        {
            return BadRequest($"Сейчас ход игрока #{gameSession.PlayerTurnId}");
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

        if (gameSession.FirstPlayerId == user.Id || gameSession.SecondPlayerId == user.Id)
        {
            return BadRequest("Вы уже в сессии");
        }

        if (gameSession.SecondPlayerId is not null)
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