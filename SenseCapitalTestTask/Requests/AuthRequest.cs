using System.ComponentModel.DataAnnotations;

namespace SenseCapitalTestTask.Requests;

public class AuthRequest
{
    [MinLength(8)]
    [MaxLength(16)]
    public string Username { get; set; }

    [MinLength(8)]
    [MaxLength(16)]
    public string Password { get; set; }
}