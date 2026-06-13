using Application.Clients.DTOs;
using Application.Shared;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Clients;

public class ClientService(DatabaseContext ctx) : IClientService
{
    public async Task RegisterIndividualClientAsync(CreateIndividualClientDto dto, CancellationToken cancellationToken)
    {
        var emailExists = await ctx.Clients.AnyAsync(c => c.Email == dto.Email, cancellationToken);
        if (emailExists)
        {
            throw new ConflictException($"Client with email  {dto.Email} already exists");
        }
        
        var peselExists = await ctx.IndividualClients.AnyAsync(c => c.Pesel == dto.Pesel, cancellationToken);
        if (peselExists)
        {
            throw new ConflictException($"Client with pesel {dto.Pesel} already exists");
        }
        
        var newClient = new IndividualClient
        {
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Pesel = dto.Pesel,
            IsDeleted = false
        };
        
        await ctx.IndividualClients.AddAsync(newClient, cancellationToken);
        await ctx.SaveChangesAsync(cancellationToken);
    }

    public async Task RegisterCompanyClientAsync(CreateCompanyClientDto dto, CancellationToken cancellationToken)
    {
        var emailExists = await ctx.Clients.AnyAsync(c => c.Email == dto.Email, cancellationToken);
        if (emailExists)
        {
            throw new ConflictException($"Client with email {dto.Email} already exists.");
        }

        var krsExists = await ctx.Companies.AnyAsync(c => c.Krs == dto.Krs, cancellationToken);
        if (krsExists)
        {
            throw new ConflictException($"Company with KRS {dto.Krs} already exists.");
        }
        
        var newCompany = new Company
        {
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            Name = dto.Name,
            Krs = dto.Krs,
            IsDeleted = false
        };
        
        await ctx.Companies.AddAsync(newCompany, cancellationToken);
        await ctx.SaveChangesAsync(cancellationToken);
    }
}