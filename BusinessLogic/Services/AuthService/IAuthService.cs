using Shared.Entities;

namespace BusinessLogic.Services.AuthService;

public interface IAuthService
{
    Task<string> Login(User user);

    Task<string> Register(User user);
}