using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitWise.Application.DTOs;
using SplitWise.Application.Interfaces.Services;

namespace SplitWise.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> MeAsync()
    {
        var user = await _userService.GetMeAsync(CurrentUserId);
        return Ok(user);
    }
    
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe(UpdateUserRequest request)
    {
         var user = await _userService.UpdateMeAsync(
            CurrentUserId,
            request);

        if (user == null)
            return NotFound();

        return Ok(user);
    }
}