using Application.Payments.DTOs;
using Application.Shared;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Payments;

public class PaymentService(DatabaseContext ctx) : IPaymentService
{
    public async Task CreateContractPaymentAsync(CreateContractPaymentDto dto, CancellationToken cancellationToken)
    {
        var contract = await ctx.Contracts
            .Include(c => c.ContractPayments)
            .Include(c => c.Status)
            .FirstOrDefaultAsync(c => c.ContractId == dto.ContractId, cancellationToken);

        if (contract is null)
        {
            throw new NotFoundException($"Contract with id {dto.ContractId} not found");
        }

        if (contract.ClientId != dto.ClientId)
        {
            throw new ConflictException($"This contract belongs to other client");
        }

        if (contract.Status.StatusName == "Signed")
        {
            throw new ConflictException($"This contract is already signed");
        }

        if (DateTime.Now > contract.EndDate)
        {
            if (contract.ContractPayments.Any())
            {
                foreach (var paymentToDelete in contract.ContractPayments)
                {
                    contract.ContractPayments.Remove(paymentToDelete);
                }
            }
            
            throw new ConflictException($"Contract's payment time is over. Previous payments refunded");
        }
        
        var sumPaid = contract.ContractPayments.Sum(p => p.Amount);
        var amountLeft = contract.Price - sumPaid;

        if (dto.Amount > amountLeft)
        {
            throw new ConflictException($"ContractPayment too high, {amountLeft} left");
        }

        var payment = new ContractPayment
        {
            ClientId = dto.ClientId,
            ContractId = dto.ContractId,
            Amount = dto.Amount,
            PaymentDate = DateTime.Now
        };
        
        await ctx.ContractPayments.AddAsync(payment, cancellationToken);

        if (sumPaid + dto.Amount == contract.Price)
        {
            var status =
                await ctx.ContractStatuses.FirstOrDefaultAsync(s => s.StatusName == "Signed", cancellationToken);

            if (status is null)
            {
                throw new Exception($"Status signed missing");
            }
            
            contract.StatusId = status.StatusId;
        }
        
        await ctx.SaveChangesAsync(cancellationToken);        
    }

    public async Task CreateSubscriptionPaymentAsync(CreateSubscriptionPaymentDto dto, CancellationToken cancellationToken)
    {
        var subscription =
            await ctx.Subscriptions.FirstOrDefaultAsync(s => s.SubscriptionId == dto.SubscriptionId, cancellationToken);

        if (subscription is null)
        {
            throw new NotFoundException($"Subscription with id {dto.SubscriptionId} not found");
        }

        var lastPayment = await ctx.SubscriptionPayments
            .Where (p => p.SubscriptionId == subscription.SubscriptionId)
            .OrderByDescending(p => p.PaymentDate)
            .FirstOrDefaultAsync(cancellationToken);
        
        

        if (lastPayment != null && lastPayment.PaymentDate >= subscription.ActiveUntil.AddDays(-subscription.PeriodInMonths))
        {
            if (DateTime.Now <= subscription.ActiveUntil)
            {
                throw new ConflictException("You have already paid for current period");
            }
        }

        if (dto.Amount != subscription.Price)
        {
            throw new ConflictException($"Subscription price must be equal to {subscription.Price}");
        }

        var payment = new SubscriptionPayment
        {
            SubscriptionId = dto.SubscriptionId,
            ClientId = dto.ClientId,
            Amount = dto.Amount,
            PaymentDate = DateTime.Now
        };
        
        await ctx.SubscriptionPayments.AddAsync(payment, cancellationToken);
        subscription.ActiveUntil = subscription.ActiveUntil.AddDays(subscription.PeriodInMonths);
        
        await ctx.SaveChangesAsync(cancellationToken);
    }
}