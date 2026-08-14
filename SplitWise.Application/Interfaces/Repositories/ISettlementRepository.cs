using SplitWise.Domain.Entities;

namespace SplitWise.Application.Interfaces.Repositories;

public interface ISettlementRepository
{
    Task<List<Settlement>> GetByGroupIdAsync(int groupId);

    Task AddAsync(Settlement settlement);

    Task SaveChangesAsync();
}