using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.Entities;

namespace SenseCapitalTestTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<string>> LogIn()
    {
        return Ok("dasdasdasd");
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<string>> Register()
    {
        
        
        return Ok("sdadsad");
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };

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