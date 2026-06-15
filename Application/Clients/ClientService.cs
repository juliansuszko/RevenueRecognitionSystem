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

    public async Task UpdateIndividualClientAsync(int id, UpdateIndividualClientDto dto, CancellationToken cancellationToken)
    {
        var individualClient = await ctx.IndividualClients.FirstOrDefaultAsync(c => c.ClientId == id, cancellationToken);

        if (individualClient == null)
        {
            throw new NotFoundException($"Individual client with id {id} not found");
        }
            individualClient.Address = dto.Address;
            individualClient.Email = dto.Email;
            individualClient.PhoneNumber = dto.PhoneNumber;
            individualClient.FirstName = dto.FirstName;
            individualClient.LastName = dto.LastName;
            
            await ctx.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateCompanyClientAsync(int id, UpdateCompanyClientDto dto, CancellationToken cancellationToken)
    {
        var company = await ctx.Companies.FirstOrDefaultAsync(c => c.ClientId == id, cancellationToken);
        if (company == null)
        {
            throw new NotFoundException($"Company with id {id} not found");
        }
        
        company.Address = dto.Address;
        company.Email = dto.Email;
        company.PhoneNumber = dto.PhoneNumber;
        company.Name = dto.Name;
            
        await ctx.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteClientAsync(int id, CancellationToken cancellationToken)
    {
        var individualClient = await ctx.IndividualClients.FirstOrDefaultAsync(c => c.ClientId == id, cancellationToken);

        if (individualClient != null)
        {
            individualClient.FirstName = "deleted";
            individualClient.LastName = "deleted";
            individualClient.Email = $"deleted{id}";
            individualClient.PhoneNumber = "000000000";
            individualClient.Address = "deleted";


            var zerosToAdd = 11 - id;
            string zeros = new string('0', zerosToAdd);
            
            individualClient.Pesel = zeros + id.ToString();
            individualClient.IsDeleted = true;
            
            await ctx.SaveChangesAsync(cancellationToken);
            return;
        }
        
        var company = await ctx.Companies.FirstOrDefaultAsync(c => c.ClientId == id, cancellationToken);
        if (company != null)
        {
            throw new ConflictException("Company data cannot be deleted");
        }
        
        throw new NotFoundException($"Client with id {id} not found");
    }
}