using SplitWise.Application.DTOs.Expense;

namespace SplitWise.Application.Interfaces.Services;

public interface IExpenseService
{
    Task<ExpenseResponse> CreateExpenseAsync(int currentUserId, int groupId, CreateExpenseRequest request);
    Task<List<ExpenseResponse>> GetGroupExpensesAsync( int currentUserId, int groupId);
}