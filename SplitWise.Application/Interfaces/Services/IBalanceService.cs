using SplitWise.Application.DTOs.Balances;

namespace SplitWise.Application.Interfaces.Services;

public interface IBalanceService
{
    Task<List<GroupBalanceResponse>> GetGroupBalancesAsync( int currentUserId, int groupId);
    Task<List<SettlementSuggestionResponse>> GetSettlementSuggestionsAsync(int currentUserId, int groupId);
}