using System.Security.Cryptography;
using System.Text;
using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace BusinessLogic.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly DatabaseContext _context;

    public AuthService(DatabaseContext context)
    {
        _context = context;
    }


    public async Task<string> Login(User user)
    {
        var userData = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(user.Username));
        
        if (userData is null)
        {
            throw new Exception("Неправильный логин или пароль");
        }

        if (!VerifyPasswordHash(userData, user.Password))
        {
            throw new Exception("Неправильный логин или пароль");
        }

        return userData.Username;
    }

    public async Task<string> Register(User user)
    {
        if (await FindByName(user.Username))
        {
            throw new Exception("Пользователь с таким именем уже существует");
        }
        
        CreatePasswordHash(user.Password, out var passwordHash, out var passwordSalt);

        user.Password = passwordHash;
        user.PasswordSalt = passwordSalt;

        var newUser = await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();

        return newUser.Entity.Username;
    }

    private Task<bool> FindByName(string username)
    {
        return _context.Users.AnyAsync(u => u.Username.Equals(username));
    }

    private void CreatePasswordHash(byte[] password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();

        passwordHash = hmac.ComputeHash(password);

        passwordSalt = hmac.Key;
    }

    private bool VerifyPasswordHash(User user, byte[] password)
    {
        using var hmac = new HMACSHA512(user.PasswordSalt);

        var hash = hmac.ComputeHash(password);

        var res = hash.SequenceEqual(user.Password);

        return res;
    }
}