using SplitWise.Application.DTOs.Expense;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Application.Interfaces.Services;
using SplitWise.Domain.Entities;
using SplitWise.Domain.Enums;

namespace SplitWise.Infrastructure.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IGroupMemberRepository _groupMemberRepository;
    
    public ExpenseService(IExpenseRepository expenseRepository, IGroupRepository groupRepository, IGroupMemberRepository groupMemberRepository)
    {
        _expenseRepository = expenseRepository;
        _groupRepository = groupRepository;
        _groupMemberRepository = groupMemberRepository;
    }
    public async Task<ExpenseResponse> CreateExpenseAsync(int currentUserId, int groupId, CreateExpenseRequest request)
    {
        await ValidateCreateExpenseRequestAsync(currentUserId, groupId, request);
        
        var expense = new Expense
        {
            Description = request.Description,
            TotalAmount = request.TotalAmount,
            ExpenseDate = request.ExpenseDate,
            SplitType = request.SplitType,
            CreatedBy = currentUserId,
            PaidBy = request.PaidBy,
            GroupId = groupId
        };
        
        CalculateShareAmount(request, expense);
       
        await _expenseRepository.AddAsync(expense);
        await _expenseRepository.SaveChangesAsync();
        
        return new ExpenseResponse
        {
            Id = expense.Id,
            Description = expense.Description,
            TotalAmount = expense.TotalAmount,
            ExpenseDate = expense.ExpenseDate,
            PaidBy = expense.PaidBy,
            GroupId = expense.GroupId,

            Splits = expense.ExpenseSplits
                .Select(x => new ExpenseSplitResponse
                {
                    UserId = x.UserId,
                    ShareAmount = x.ShareAmount
                })
                .ToList()
        };
    }

    public async Task<List<ExpenseResponse>> GetGroupExpensesAsync(int currentUserId, int groupId)
    { var group = await _groupRepository
            .GetGroupByIdAsync(groupId, currentUserId);

        if (group == null)
            throw new Exception(
                "No group found or you are not a member of this group.");

        var expenses = await _expenseRepository
            .GetByGroupIdAsync(groupId);

        return expenses.Select(expense => new ExpenseResponse
        {
            Id = expense.Id,
            Description = expense.Description,
            TotalAmount = expense.TotalAmount,
            ExpenseDate = expense.ExpenseDate,
            PaidBy = expense.PaidBy,
            GroupId = expense.GroupId,

            Splits = expense.ExpenseSplits
                .Select(split => new ExpenseSplitResponse
                {
                    UserId = split.UserId,
                    ShareAmount = split.ShareAmount
                })
                .ToList()
        }).ToList();
    }

    private async Task ValidateCreateExpenseRequestAsync(int currentUserId, int groupId, CreateExpenseRequest request)
    {
        if (request.TotalAmount <= 0)
            throw new ArgumentException("Expense amount must be greater than zero.");
        if (request.Shares == null || request.Shares.Count == 0)
            throw new ArgumentException(
                "At least one user is required.");
        if (request.SplitType == SplitType.Exact)
            ValidateExactSplit(request);
        
        if (request.SplitType == SplitType.Percentage)
            ValidatePercentageSplit(request);
        
        var group = await _groupRepository.GetGroupByIdAsync(groupId, currentUserId);
        if (group==null)
            throw new Exception("No group found or you are not a member of this group.");
        var areActiveMembers = await _groupMemberRepository.AreActiveMembersAsync(groupId,
            request.Shares.Select(x=>x.UserId).Append(request.PaidBy).Distinct().ToList());
        if (!areActiveMembers)
            throw new InvalidOperationException(
                "All users must be active members of the group.");
    }
    private void ValidateExactSplit(CreateExpenseRequest request)
    {
        var shares = request.Shares;

        if (shares.Any(x => x.Amount <= 0))
            throw new ArgumentException(
                "Share amount must be greater than zero.");

        var userIds = shares
            .Select(x => x.UserId)
            .ToList();

        if (userIds.Count != userIds.Distinct().Count())
            throw new ArgumentException(
                "A user cannot have multiple shares in the same expense.");

        var totalShares = shares.Sum(x => x.Amount);

        if (totalShares != request.TotalAmount)
            throw new ArgumentException(
                "The total of all shares must equal the expense amount.");
    }
    
    private void ValidatePercentageSplit(CreateExpenseRequest request)
    {
        var shares = request.Shares;

        if (shares.Any(x => x.Amount <= 0))
            throw new ArgumentException(
                "Percentage must be greater than zero.");

        if (shares.Select(x => x.UserId).Distinct().Count() != shares.Count)
            throw new ArgumentException(
                "A user cannot have multiple shares in the same expense.");

        var totalPercentage = shares.Sum(x => x.Amount);

        if (totalPercentage != 100)
            throw new ArgumentException(
                "The total percentage must equal 100.");
    }
    private void CalculateShareAmount(CreateExpenseRequest request, Expense expense)
    {
        switch (request.SplitType)
        {
            case SplitType.Equal:
                var userIds = request.Shares.Select(x=>x.UserId).Distinct().ToList();
                
                var shareAmount = Math.Round(request.TotalAmount / userIds.Count, 2);

                var allocatedAmount = 0m;
                for (var i = 0; i < userIds.Count; i++)
                {
                    var amount = i == userIds.Count - 1
                        ? request.TotalAmount - allocatedAmount
                        : shareAmount;

                    expense.ExpenseSplits.Add(new ExpenseSplit
                    {
                        UserId = userIds[i],
                        ShareAmount = amount
                    });
                    allocatedAmount += amount;
                }
                break;
            case SplitType.Exact:
                foreach (var share in request.Shares)
                {
                    expense.ExpenseSplits.Add(new ExpenseSplit
                    {
                        UserId = share.UserId,
                        ShareAmount = share.Amount
                    });
                }
                break;
            case SplitType.Percentage:
                var percentageShares = request.Shares;
                var allocatedPercentageAmount = 0m;
                for (var i = 0; i < percentageShares.Count; i++)
                {
                    var share = percentageShares[i];

                    var amount = i == percentageShares.Count - 1
                        ? request.TotalAmount - allocatedPercentageAmount
                        : Math.Round(
                            request.TotalAmount * share.Amount / 100m,
                            2);

                    expense.ExpenseSplits.Add(new ExpenseSplit
                    {
                        UserId = share.UserId,
                        ShareAmount = amount
                    });

                    allocatedPercentageAmount += amount;
                }

                break;
            default:
                throw new NotImplementedException();
        }
    }
}