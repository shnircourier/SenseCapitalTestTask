using Shared.Enums;

namespace Shared.Entities;

public class Player
{
    public int UserId { get; set; }
    
    public User User { get; set; }

    public PlayerSide Side { get; set; }
}