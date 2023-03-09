using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace BusinessLogic.Services.UserService;

public class UserService : IUserService
{
    private readonly DatabaseContext _context;

    public UserService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<User> GetByName(string name)
    {
        return await _context.Users.FirstAsync(u => u.Username == name);
    }
}