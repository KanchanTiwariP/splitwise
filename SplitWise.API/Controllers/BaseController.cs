using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SplitWise.API.Controllers;

[ApiController]
[Authorize]
public class BaseController : ControllerBase
{
    protected int CurrentUserId
    {
        get
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException();

            return int.Parse(userId);
        }
    }
}