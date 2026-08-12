using Microsoft.AspNetCore.Mvc;
using SplitWise.Application.DTOs.Expense;
using SplitWise.Application.Interfaces.Services;

namespace SplitWise.API.Controllers;

[Route("api/[controller]")]
public class ExpenseController :BaseController
{
    private readonly IExpenseService _expenseService;
    public ExpenseController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense(int groupId, CreateExpenseRequest request)
    {
        var expense =  await _expenseService.CreateExpenseAsync(CurrentUserId, groupId, request);
        return Ok(expense);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetGroupExpenses(int groupId)
    {
        var expenses = await _expenseService
            .GetGroupExpensesAsync(CurrentUserId, groupId);
        return Ok(expenses);
    }
}