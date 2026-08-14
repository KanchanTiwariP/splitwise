using Microsoft.AspNetCore.Mvc;
using SplitWise.Application.Interfaces.Services;

namespace SplitWise.API.Controllers;

[Route("api/groups/{groupId:int}/balances")]
public class BalanceController: BaseController
{
    private readonly IBalanceService _balanceService;

    public BalanceController(IBalanceService balanceService)
    {
        _balanceService = balanceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGroupBalances(int groupId)
    {
        var balances = await _balanceService
            .GetGroupBalancesAsync(CurrentUserId, groupId);

        return Ok(balances);
    }
    
    [HttpGet("settlements")]
    public async Task<IActionResult> GetSettlementSuggestions(int groupId)
    {
        var settlements = await _balanceService
            .GetSettlementSuggestionsAsync(
                CurrentUserId,
                groupId);

        return Ok(settlements);
    }
}