using Application.Contracts.DTOs;
using Application.Shared;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Contracts;

public class ContractService(DatabaseContext ctx) : IContractService
{
    public async Task CreateContractAsync(CreateContractDto dto, CancellationToken cancellationToken)
    {
        var daysForPayment = (dto.EndDate - DateTime.Now).TotalDays;
        if (daysForPayment < 3 || daysForPayment > 30)
        {
            throw new ConflictException("Time to pay for the contract must be between 3-30 days");
        }

        var clientExists = await ctx.Clients.AnyAsync(c => c.ClientId == dto.ClientId, cancellationToken);
        if (!clientExists)
        {
            throw new NotFoundException($"Client with id {dto.ClientId} not found");
        }

        var hasActiveOrSignedContract = await ctx.Contracts.AnyAsync(c => c.ClientId == dto.ClientId
                                && c.SoftwareId == dto.SoftwareId
                                && (c.Status.StatusName == "Signed" || c.Status.StatusName == "Active"),
            cancellationToken
        );
        if (hasActiveOrSignedContract)
        {
            throw new ConflictException("Client already has an active or signed contract for this software");
        }
        
        var software = await ctx.Softwares
            .Include(s => s.Discounts)
            .FirstOrDefaultAsync(s => s.SoftwareId == dto.SoftwareId, cancellationToken);

        if (software == null)
        {
            throw new NotFoundException($"Software with id {dto.SoftwareId} not found");
        }

        var maxDiscount = software.Discounts
            .Where(d => d.DateFrom <= DateTime.Now && d.DateTo >= DateTime.Now)
            .Max(d => (decimal?)d.Value) ?? 0m;
        
        var isReturningClient = await ctx.Contracts.AnyAsync(c => c.ClientId == dto.ClientId && c.Status.StatusName == "Signed", cancellationToken);

        var additionalDiscount = isReturningClient ? 5m : 0m;

        var totalDiscount = (maxDiscount + additionalDiscount) / 100;
        var priceAfterDiscount = software.BasePrice - (software.BasePrice * totalDiscount);
        
        var extraYearsCost = (dto.YearsOfSupport - 1) * 1000m;
        
        var finalPrice = priceAfterDiscount + extraYearsCost;
        
        var contractStatus = await ctx.ContractStatuses.FirstOrDefaultAsync(s => s.StatusName == "Active", cancellationToken);
        if (contractStatus == null)
        {
            throw new Exception("Status Active missing");
        }

        var contract = new Contract
        {
            ClientId = dto.ClientId,
            SoftwareId = dto.SoftwareId,
            SoftwareVersion = software.LatestVersion,
            StartDate = DateTime.Now,
            EndDate = dto.EndDate,
            YearsOfSupport = dto.YearsOfSupport,
            Price = finalPrice,
            StatusId = contractStatus.StatusId
        };
        
        await ctx.Contracts.AddAsync(contract, cancellationToken);
        await ctx.SaveChangesAsync(cancellationToken);
    }
}