using SplitWise.Application.DTOs.Settlements;
using SplitWise.Application.Exceptions;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Application.Interfaces.Services;
using SplitWise.Domain.Entities;

namespace SplitWise.Infrastructure.Services;

public class SettlementService : ISettlementService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IGroupMemberRepository _groupMemberRepository;
    private readonly ISettlementRepository _settlementRepository;
    private readonly IGroupAccessService _groupAccessService;

    public SettlementService(IExpenseRepository expenseRepository,
        ISettlementRepository settlementRepository, IGroupMemberRepository groupMemberRepository, IGroupAccessService groupAccessService)
    {
        _expenseRepository = expenseRepository;
        _settlementRepository = settlementRepository;
        _groupMemberRepository = groupMemberRepository;
        _groupAccessService = groupAccessService;
    }

    public async Task CreateSettlementAsync(int currentUserId, int groupId, CreateSettlementRequest request)
    {
        ValidateRequest(currentUserId, request);

        await _groupAccessService
            .GetGroupAsync(groupId, currentUserId);

        var userIds = new[]
        {
            request.PayerId,
            request.ReceiverId
        };

        var areActiveMembers =
            await _groupMemberRepository.AreActiveMembersAsync(
                groupId,
                userIds);

        if (!areActiveMembers)
            throw new ValidationException(
                "Both users must be active members of the group.");

        var expenses = await _expenseRepository
            .GetByGroupIdAsync(groupId);

        var settlements = await _settlementRepository
            .GetByGroupIdAsync(groupId);

        var amountOwed = CalculateAmountOwed(
            request.PayerId,
            request.ReceiverId,
            expenses,
            settlements);

        if (request.Amount > amountOwed)
            throw new ValidationException(
                $"Settlement amount cannot exceed the outstanding debt of {amountOwed}.");

        var settlement = new Settlement
        {
            PayerId = request.PayerId,
            ReceiverId = request.ReceiverId,
            GroupId = groupId,
            Amount = request.Amount,
            SettlementDate = request.SettlementDate
        };

        await _settlementRepository.AddAsync(settlement);
        await _settlementRepository.SaveChangesAsync();
    }

    private static void ValidateRequest(
        int currentUserId,
        CreateSettlementRequest request)
    {
        if (currentUserId != request.PayerId &&
            currentUserId != request.ReceiverId)
            throw new ForbiddenException(
                "Only the payer or receiver can record this settlement.");

        if (request.PayerId == request.ReceiverId)
            throw new ValidationException(
                "Payer and receiver must be different users.");

        if (request.Amount <= 0)
            throw new ValidationException(
                "Settlement amount must be greater than zero.");
    }

    private static decimal CalculateAmountOwed(
        int payerId,
        int receiverId,
        List<Expense> expenses,
        List<Settlement> settlements)
    {
        var payerOwes = 0m;
        var receiverOwes = 0m;

        foreach (var expense in expenses)
        {
            if (expense.PaidBy == receiverId)
            {
                var payerShare = expense.ExpenseSplits
                    .Where(x => x.UserId == payerId)
                    .Sum(x => x.ShareAmount);

                payerOwes += payerShare;
            }

            if (expense.PaidBy == payerId)
            {
                var receiverShare = expense.ExpenseSplits
                    .Where(x => x.UserId == receiverId)
                    .Sum(x => x.ShareAmount);

                receiverOwes += receiverShare;
            }
        }

        foreach (var settlement in settlements)
        {
            if (settlement.PayerId == payerId &&
                settlement.ReceiverId == receiverId)
                payerOwes -= settlement.Amount;

            if (settlement.PayerId == receiverId &&
                settlement.ReceiverId == payerId)
                receiverOwes -= settlement.Amount;
        }

        return Math.Max(0, payerOwes - receiverOwes);
    }
}