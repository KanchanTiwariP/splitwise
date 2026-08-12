using SplitWise.Domain.Entities;

namespace SplitWise.Application.Interfaces.Repositories;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(int expenseId);
    Task AddAsync(Expense expense);
    Task<List<Expense>> GetByGroupIdAsync(int groupId);
    Task SaveChangesAsync();
}