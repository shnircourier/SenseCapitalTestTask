using Shared.Entities;

namespace BusinessLogic.Services.GameSessionService;

public interface IGameSessionService
{
    Task<List<GameSession>> Get();

    Task<GameSession> Get(int sessionId);

    Task<GameSession> CreateSession(GameSession gameSession);

    Task<GameSession> MakeMove(GameSession gameSession, int x, int y, int userId);

    Task<GameSession> JoinToSession(int sessionId, int userId);
}