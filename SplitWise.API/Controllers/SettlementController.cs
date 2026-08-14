using Microsoft.AspNetCore.Mvc;
using SplitWise.Application.DTOs.Settlements;
using SplitWise.Application.Interfaces.Services;

namespace SplitWise.API.Controllers;

[Route("api/groups/{groupId:int}/settlements")]
public class SettlementController : BaseController
{
    private readonly ISettlementService _settlementService;

    public SettlementController(ISettlementService settlementService)
    {
        _settlementService = settlementService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSettlement(
        int groupId,
        CreateSettlementRequest request)
    {
        await _settlementService.CreateSettlementAsync(
            CurrentUserId,
            groupId,
            request);

        return Ok();
    }
}