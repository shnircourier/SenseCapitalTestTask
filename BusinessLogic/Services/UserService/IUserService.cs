using Shared.Entities;

namespace BusinessLogic.Services.UserService;

public interface IUserService
{
    Task<User> GetByName(string name);
}