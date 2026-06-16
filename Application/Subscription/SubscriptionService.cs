using Application.Shared;
using Application.Subscription.DTOs;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Subscription;

public class SubscriptionService(DatabaseContext ctx) : ISubscriptionService
{
    public async Task CreateSubscriptionAsync(CreateSubscriptionDto dto, CancellationToken cancellationToken)
    {
        var client = await ctx.Clients.FirstOrDefaultAsync(c => c.ClientId == dto.ClientId, cancellationToken);
        if (client is null)
        {
            throw new NotFoundException($"Client with id {dto.ClientId} does not exist");
        }
        
        var software = await ctx.Softwares
            .Include(s => s.Discounts)
            .FirstOrDefaultAsync(s => s.SoftwareId == dto.SoftwareId, cancellationToken);

        if (software is null)
        {
            throw new NotFoundException($"Software with id {dto.SoftwareId} does not exist");
        }
        
        var isReturningClient = await ctx.Contracts.AnyAsync(c => c.ClientId == dto.ClientId && c.Status.StatusName == "Signed", cancellationToken) ||
                            await ctx.Subscriptions.AnyAsync(s => s.ClientId == dto.ClientId, cancellationToken);
        
        var additionalDiscount = isReturningClient ? 5m : 0m;
        
        var maxDiscount = software.Discounts
            .Where(d => d.DateFrom <= DateTime.UtcNow && d.DateTo >= DateTime.UtcNow)
            .Max(d => (decimal?)d.Value) ?? 0m;
        
        var totalDiscount = (maxDiscount + additionalDiscount) / 100;
        var priceForPeriod = software.BasePrice * dto.PeriodInMonths;
        var priceAfterAllDiscounts = priceForPeriod - (priceForPeriod * totalDiscount);
        var priceAfterLoyalDiscount = priceForPeriod - (priceForPeriod * (additionalDiscount/100));
        
        var subscription = new Domain.Entities.Subscription
        {
            Name = dto.Name,
            PeriodInMonths = dto.PeriodInMonths,
            ClientId = dto.ClientId,
            SoftwareId = dto.SoftwareId,
            Price = priceAfterLoyalDiscount,
            ActiveUntil = DateTime.UtcNow.AddMonths(dto.PeriodInMonths)
        };
        
        await ctx.Subscriptions.AddAsync(subscription, cancellationToken);

        var firstPayment = new SubscriptionPayment
        {
            Subscription = subscription,
            ClientId = dto.ClientId,
            Amount = priceAfterAllDiscounts,
            PaymentDate = DateTime.UtcNow,
        };
        
        await ctx.SubscriptionPayments.AddAsync(firstPayment, cancellationToken);
        await ctx.SaveChangesAsync(cancellationToken);
    }
}