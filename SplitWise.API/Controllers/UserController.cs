using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitWise.Application.DTOs;
using SplitWise.Application.Interfaces.Services;

namespace SplitWise.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> MeAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return NotFound();
        var user = await _userService.GetMeAsync(int.Parse(userId));
        return Ok(user);
    }
    
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe(UpdateUserRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var user = await _userService.UpdateMeAsync(
            int.Parse(userId),
            request);

        if (user == null)
            return NotFound();

        return Ok(user);
    }
}