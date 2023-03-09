using System.ComponentModel.DataAnnotations;

namespace SenseCapitalTestTask.Requests;

public class UpdateGameSessionRequest
{
    [Range(0, 2)]
    public int MatrixX { get; set; }

    [Range(0, 2)]
    public int MatrixY { get; set; }

    public int SessionId { get; set; }
}