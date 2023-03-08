using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLogic.Services.AuthService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SenseCapitalTestTask.Requests;
using Shared.Entities;

namespace SenseCapitalTestTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IAuthService _authService;

    public AuthController(IConfiguration config, IAuthService authService)
    {
        _config = config;
        _authService = authService;
    }

    [HttpPost]
    [Route("LogIn")]
    public async Task<ActionResult<string>> LogIn(UserRequest request)
    {
        try
        {
            var username = await _authService.Login(new User
            {
                Password = Encoding.UTF8.GetBytes(request.Password),
                Username = request.Username
            });
            
            var token = CreateToken(username);
            
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult<string>> Register(UserRequest request)
    {
        try
        {
            var username = await _authService.Register(new User
            {
                Password = Encoding.UTF8.GetBytes(request.Password),
                Username = request.Username
            });

            var token = CreateToken(username);
            
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
        
    }

    private string CreateToken(string username)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims, 
            expires: DateTime.Now.AddSeconds(int.Parse(_config["JWT:ExpiresSeconds"])),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}