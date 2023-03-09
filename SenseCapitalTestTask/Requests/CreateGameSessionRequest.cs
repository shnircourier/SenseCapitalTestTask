using Shared.Enums;

namespace SenseCapitalTestTask.Requests;

public class CreateGameSessionRequest
{
    public PlayerSide PlayerSide { get; set; } = PlayerSide.Crosses;
}