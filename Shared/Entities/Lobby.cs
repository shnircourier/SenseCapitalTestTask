namespace Shared.Entities;

public class Lobby
{
    public int Id { get; set; }

    public Player FirstPlayer { get; set; }

    public Player SecondPlayer { get; set; }
}