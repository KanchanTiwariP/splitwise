using Microsoft.EntityFrameworkCore;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Domain.Entities;
using SplitWise.Infrastructure.Persistence;
namespace SplitWise.Infrastructure.Repositories;

public class SettlementRepository: ISettlementRepository
{
    private readonly AppDbContext _context;

    public SettlementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Settlement>> GetByGroupIdAsync(int groupId)
    {
        return await _context.Settlements
            .Where(x => x.GroupId == groupId)
            .ToListAsync();
    }

    public async Task AddAsync(Settlement settlement)
    {
        await _context.Settlements.AddAsync(settlement);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}