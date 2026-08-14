using SplitWise.Application.DTOs.Settlements;

namespace SplitWise.Application.Interfaces.Services;

public interface ISettlementService
{
    Task CreateSettlementAsync(int currentUserId, int groupId, CreateSettlementRequest request);
}