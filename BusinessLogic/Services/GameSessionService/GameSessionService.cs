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

    public async Task<GameSession> CreateSession(Player player)
    {
        var gameSession = new GameSession
        {
            FirstPlayer = player,
            GameBoard = new[]
            {
                new[] { CellStatus.Empty, CellStatus.Empty, CellStatus.Empty },
                new[] { CellStatus.Empty, CellStatus.Empty, CellStatus.Empty },
                new[] { CellStatus.Empty, CellStatus.Empty, CellStatus.Empty }
            }
        };

        return await _repository.Create(gameSession);
    }

    public async Task<GameSession> MakeMove(GameSession gameSession, int x, int y, int userId)
    {
        var value = gameSession.PlayerTurn.Side == PlayerSide.Crosses ? CellStatus.Cross : CellStatus.Zero;
        
        var newBoard = ChangeCellValue(gameSession.GameBoard, x, y, value);

        if (IsDraw(newBoard))
        {
            gameSession.IsGameEnded = true;
            gameSession.GameBoard = newBoard;
            
            return await _repository.Update(gameSession);
        }

        if (IsGameEnd(newBoard, value))
        {
            gameSession.IsGameEnded = true;
            gameSession.GameBoard = newBoard;
            gameSession.Winner = gameSession.PlayerTurn;
            
            return await _repository.Update(gameSession);
        }

        gameSession.GameBoard = newBoard;
        gameSession.PlayerTurn = gameSession.FirstPlayer.UserId == gameSession.PlayerTurn.UserId
            ? gameSession.SecondPlayer
            : gameSession.FirstPlayer;
        
        return await _repository.Update(gameSession);

    }

    public async Task<GameSession> JoinToSession(int sessionId, int userId)
    {
        var gameSession = await _repository.Get(sessionId);

        if (gameSession.FirstPlayer.UserId == userId)
        {
            throw new Exception("Игрок уже в сессии");
        }

        gameSession.SecondPlayer = new Player
        {
            UserId = userId,
            Side = gameSession.FirstPlayer.Side == PlayerSide.Crosses ? PlayerSide.Zeroes : PlayerSide.Crosses
        };
        gameSession.PlayerTurn = gameSession.FirstPlayer.Side == PlayerSide.Crosses
            ? gameSession.FirstPlayer
            : gameSession.SecondPlayer;

        return await _repository.Update(gameSession);
    }

    private CellStatus[][] ChangeCellValue(CellStatus[][] board, int x, int y, CellStatus value)
    {
        if (board[x][y] == CellStatus.Empty)
        {
            board[x][y] = value;

            return board;
        }

        if (board[x][y] == value)
        {
            throw new Exception("Данная ячейка уже вами занята");
        }

        throw new Exception("Данная ячейка уже занята вашим оппонентом");
    }

    private bool IsDraw(CellStatus[][] board)
    {
        return !board.Any(i => i.Any(j => j == CellStatus.Empty));
    }

    private bool IsGameEnd(CellStatus[][] board, CellStatus value)
    {
        var firstExp = board.Aggregate(false, (current, i) => current | i.All(j => j == value));

        var secondExp = board.Aggregate(false, (current, b) => current & b[0] == value);
        
        var thirdExp = board.Aggregate(false, (current, b) => current & b[1] == value);
        
        var fourthExp = board.Aggregate(false, (current, b) => current & b[2] == value);

        var fifthExp = board[0][0] == value && board[1][1] == value && board[2][2] == value;

        var sixthExp = board[0][2] == value && board[1][1] == value && board[2][0] == value;

        return firstExp || secondExp || thirdExp || fourthExp || fifthExp || sixthExp;
    }
}