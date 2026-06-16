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
            .Include(c => c.Payments)
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
            throw new ConflictException($"Contract's payment time is over");
        }
        
        var sumPaid = contract.Payments.Sum(p => p.Amount);
        var amountLeft = contract.Price - sumPaid;

        if (dto.Amount < amountLeft)
        {
            throw new ConflictException($"Payment too high, {amountLeft} left");
        }

        var payment = new Payment
        {
            ClientId = dto.ClientId,
            ContractId = dto.ContractId,
            Amount = dto.Amount,
            PaymentDate = DateTime.Now
        };
        
        await ctx.Payments.AddAsync(payment, cancellationToken);

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

    public Task CreateSubscriptionPaymentAsync(CreateSubscriptionPaymentDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}