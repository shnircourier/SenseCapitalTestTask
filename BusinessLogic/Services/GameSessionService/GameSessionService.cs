using BusinessLogic.Helpers;
using Data;
using Shared.Entities;
using Shared.Enums;

namespace BusinessLogic.Services.GameSessionService;

public class GameSessionService : IGameSessionService
{
    private readonly IRepository<GameSession> _repository;

    public GameSessionService(IRepository<GameSession> repository)
    {
        _repository = repository;
    }
    
    public async Task<List<GameSession>> Get()
    {
        return await _repository.Get();
    }

    public async Task<GameSession> Get(int sessionId)
    {
        return await _repository.Get(sessionId);
    }

    public async Task<GameSession> CreateSession(GameSession gameSession)
    {
        var newBoard = new[]
        {
            new[] { CellStatus.Empty, CellStatus.Empty, CellStatus.Empty },
            new[] { CellStatus.Empty, CellStatus.Empty, CellStatus.Empty },
            new[] { CellStatus.Empty, CellStatus.Empty, CellStatus.Empty }
        };

        gameSession.GameBoard = StringHelper.GetStringFromBoard(newBoard);

        return await _repository.Create(gameSession);
    }

    public async Task<GameSession> MakeMove(GameSession gameSession, int x, int y, int userId)
    {
        var userSide = gameSession.FirstPlayerId == userId && gameSession.PlayerTurnId == userId
            ? gameSession.FirstPlayerSide
            : gameSession.SecondPlayerSide;
        
        var value = userSide == PlayerSide.Crosses ? CellStatus.Cross : CellStatus.Zero;
        
        var newBoard = ChangeCellValue(gameSession.GameBoard, x, y, value);

        if (IsGameEnd(newBoard, value))
        {
            gameSession.IsGameEnded = true;
            gameSession.GameBoard = newBoard;
            gameSession.WinnerId = gameSession.PlayerTurnId;
            
            return await _repository.Update(gameSession);
        }
        
        if (IsDraw(newBoard))
        {
            gameSession.IsGameEnded = true;
            gameSession.GameBoard = newBoard;
            
            return await _repository.Update(gameSession);
        }

        gameSession.GameBoard = newBoard;
        gameSession.PlayerTurnId = gameSession.FirstPlayerId == gameSession.PlayerTurnId
            ? gameSession.SecondPlayerId
            : gameSession.FirstPlayerId;
        
        return await _repository.Update(gameSession);
    }

    public async Task<GameSession> JoinToSession(int sessionId, int userId)
    {
        var gameSession = await _repository.Get(sessionId);

        if (gameSession.FirstPlayerId == userId)
        {
            throw new Exception("Игрок уже в сессии");
        }

        gameSession.SecondPlayerId = userId;
        gameSession.SecondPlayerSide = gameSession.FirstPlayerSide == PlayerSide.Crosses ? PlayerSide.Zeroes : PlayerSide.Crosses;
        
        gameSession.PlayerTurnId = gameSession.FirstPlayerSide == PlayerSide.Crosses
            ? gameSession.FirstPlayerId
            : gameSession.SecondPlayerId;

        return await _repository.Update(gameSession);
    }

    private string ChangeCellValue(string board, int x, int y, CellStatus value)
    {
        var newBoard = StringHelper.GetBoardFromString(board);
        
        if (newBoard[x][y] == CellStatus.Empty)
        {
            newBoard[x][y] = value;

            return StringHelper.GetStringFromBoard(newBoard);
        }

        if (newBoard[x][y] == value)
        {
            throw new Exception("Данная ячейка уже вами занята");
        }

        throw new Exception("Данная ячейка уже занята вашим оппонентом");
    }

    private bool IsDraw(string board)
    {
        var boardFromStr = StringHelper.GetBoardFromString(board);
        
        return !boardFromStr.Any(i => i.Any(j => j == CellStatus.Empty));
    }

    private bool IsGameEnd(string board, CellStatus value)
    {
        var boardFromStr = StringHelper.GetBoardFromString(board);
        
        var firstExp = boardFromStr.Aggregate(false, (current, i) => current | i.All(j => j == value));

        var secondExp = boardFromStr.Aggregate(false, (current, b) => current & b[0] == value);
        
        var thirdExp = boardFromStr.Aggregate(false, (current, b) => current & b[1] == value);
        
        var fourthExp = boardFromStr.Aggregate(false, (current, b) => current & b[2] == value);

        var fifthExp = boardFromStr[0][0] == value && boardFromStr[1][1] == value && boardFromStr[2][2] == value;

        var sixthExp = boardFromStr[0][2] == value && boardFromStr[1][1] == value && boardFromStr[2][0] == value;

        return firstExp || secondExp || thirdExp || fourthExp || fifthExp || sixthExp;
    }
}