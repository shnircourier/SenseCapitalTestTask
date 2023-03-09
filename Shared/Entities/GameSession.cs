using Shared.Enums;

namespace Shared.Entities;

public class GameSession
{
    public int Id { get; set; }

    public int FirstPlayerId { get; set; }

    public PlayerSide FirstPlayerSide { get; set; }

    public int? SecondPlayerId { get; set; }

    public PlayerSide SecondPlayerSide { get; set; }

    public int? PlayerTurnId { get; set; }

    public bool IsGameEnded { get; set; }

    public int? WinnerId { get; set; }

    public string GameBoard { get; set; }
}

    