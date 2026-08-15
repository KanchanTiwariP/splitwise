using SplitWise.Application.DTOs.Balances;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Application.Interfaces.Services;

namespace SplitWise.Infrastructure.Services;

public class BalanceService : IBalanceService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly ISettlementRepository _settlementRepository;
    private readonly IGroupAccessService _groupAccessService;
    public BalanceService(
        IGroupRepository groupRepository,
        IExpenseRepository expenseRepository,
        ISettlementRepository settlementRepository,
        IGroupAccessService groupAccessService)
    {
        _groupRepository = groupRepository;
        _expenseRepository = expenseRepository;
        _settlementRepository = settlementRepository;
        _groupAccessService = groupAccessService;
    }

    public async Task<List<GroupBalanceResponse>> GetGroupBalancesAsync(
        int currentUserId,
        int groupId)
    {
         await _groupAccessService
            .GetGroupAsync(groupId, currentUserId);

        var expenses = await _expenseRepository
            .GetByGroupIdAsync(groupId);
       
        var settlements = await _settlementRepository
            .GetByGroupIdAsync(groupId);

        var balances = new Dictionary<int, GroupBalanceResponse>();

        foreach (var expense in expenses)
        {
            if (!balances.ContainsKey(expense.PaidBy))
            {
                balances[expense.PaidBy] = new GroupBalanceResponse
                {
                    UserId = expense.PaidBy
                };
            }

            balances[expense.PaidBy].TotalPaid += expense.TotalAmount;

            foreach (var split in expense.ExpenseSplits)
            {
                if (!balances.ContainsKey(split.UserId))
                {
                    balances[split.UserId] = new GroupBalanceResponse
                    {
                        UserId = split.UserId
                    };
                }

                balances[split.UserId].TotalShare += split.ShareAmount;
            }
        }

        foreach (var balance in balances.Values)
        {
            balance.Balance =
                balance.TotalPaid - balance.TotalShare;
        }
        
        // Apply settlements
        foreach (var settlement in settlements)
        {
            if (!balances.ContainsKey(settlement.PayerId))
            {
                balances[settlement.PayerId] = new GroupBalanceResponse
                {
                    UserId = settlement.PayerId
                };
            }

            if (!balances.ContainsKey(settlement.ReceiverId))
            {
                balances[settlement.ReceiverId] = new GroupBalanceResponse
                {
                    UserId = settlement.ReceiverId
                };
            }

            balances[settlement.PayerId].Balance += settlement.Amount;
            balances[settlement.ReceiverId].Balance -= settlement.Amount;
        }

        return balances.Values
            .OrderBy(x => x.UserId)
            .ToList();
    }
    
    public async Task<List<SettlementSuggestionResponse>>
        GetSettlementSuggestionsAsync(
            int currentUserId,
            int groupId)
    {
        var balances = await GetGroupBalancesAsync(
            currentUserId,
            groupId);

        var creditors = balances
            .Where(x => x.Balance > 0)
            .Select(x => new
            {
                UserId = x.UserId,
                Amount = x.Balance
            })
            .ToList();

        var debtors = balances
            .Where(x => x.Balance < 0)
            .Select(x => new
            {
                UserId = x.UserId,
                Amount = -x.Balance
            })
            .ToList();

        var settlements = new List<SettlementSuggestionResponse>();

        var creditorIndex = 0;
        var debtorIndex = 0;

        while (creditorIndex < creditors.Count &&
               debtorIndex < debtors.Count)
        {
            var creditor = creditors[creditorIndex];
            var debtor = debtors[debtorIndex];

            var amount = Math.Min(
                creditor.Amount,
                debtor.Amount);

            settlements.Add(new SettlementSuggestionResponse
            {
                FromUserId = debtor.UserId,
                ToUserId = creditor.UserId,
                Amount = amount
            });

            creditors[creditorIndex] = new
            {
                creditor.UserId,
                Amount = creditor.Amount - amount
            };

            debtors[debtorIndex] = new
            {
                debtor.UserId,
                Amount = debtor.Amount - amount
            };

            if (creditors[creditorIndex].Amount == 0)
                creditorIndex++;

            if (debtors[debtorIndex].Amount == 0)
                debtorIndex++;
        }

        return settlements;
    }
}