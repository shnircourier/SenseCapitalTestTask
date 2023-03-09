using Shared.Enums;

namespace Shared.Entities;

public class GameSession
{
    public int Id { get; set; }

    public Player FirstPlayer { get; set; }

    public Player? SecondPlayer { get; set; }

    public Player? PlayerTurn { get; set; }

    public bool IsGameEnded { get; set; }

    public Player? Winner { get; set; }

    public CellStatus[][] GameBoard { get; set; }
}

    