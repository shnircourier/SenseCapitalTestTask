using Shared.Enums;

namespace Shared.Entities;

public class Session
{
    public int Id { get; set; }

    public Lobby Lobby { get; set; }

    public bool IsEnded { get; set; }

    public Player? Winner { get; set; }

    public CellStatus[][] BoardStatus { get; set; }
}