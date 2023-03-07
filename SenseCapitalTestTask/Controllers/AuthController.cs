using Microsoft.AspNetCore.Mvc;

namespace SenseCapitalTestTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost]
    public Task<ActionResult<string>> LogIn()
    {
        
    }

    [HttpPost]
    public Task<ActionResult<string>> Register()
    {
        
    }
}