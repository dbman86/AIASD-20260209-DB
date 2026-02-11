using Microsoft.AspNetCore.Mvc;
using PostHubAPI.Dtos.User;
using PostHubAPI.Services.Interfaces;

namespace PostHubAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var token = await userService.Register(dto);
            return Ok(token);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var token = await userService.Login(dto);
            return Ok(token);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
