using Microsoft.EntityFrameworkCore;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Domain.Entities;
using SplitWise.Infrastructure.Persistence;

namespace SplitWise.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly AppDbContext _context;
    public ExpenseRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Expense?> GetByIdAsync(int expenseId)
    {
        return await _context.Expenses
            .Include(x => x.Payer)
            .Include(x => x.ExpenseSplits)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == expenseId);
    }

    public async Task AddAsync(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
    }

    public async Task<List<Expense>> GetByGroupIdAsync(int groupId)
    {
        return await _context.Expenses
            .Where(x => x.GroupId == groupId)
            .Include(x => x.ExpenseSplits)
            .OrderByDescending(x => x.ExpenseDate)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
       await _context.SaveChangesAsync();
    }
}