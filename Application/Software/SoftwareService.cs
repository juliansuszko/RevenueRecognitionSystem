using Application.Shared;
using Application.Software.DTOs;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Software;

public class SoftwareService(DatabaseContext ctx) : ISoftwareService
{
    public async Task AddSoftwareAsync(CreateSoftwareDto dto, CancellationToken cancellationToken)
    {
        var categoryExists = await ctx.SoftwareCategories.AnyAsync(c => c.CategoryId == dto.CategoryId, cancellationToken);

        if (!categoryExists)
        {
            throw new NotFoundException($"Software category with ID {dto.CategoryId} was not found");
        }
        
        var softwareExists = await ctx.Softwares.AnyAsync(s => s.Name == dto.Name, cancellationToken);
        if (softwareExists)
        {
            throw new ConflictException($"Software with name '{dto.Name}' already exists");
        }

        var newSoftware = new Domain.Entities.Software
        {
            Name = dto.Name,
            Description = dto.Description,
            LatestVersion = dto.LatestVersion,
            CategoryId = dto.CategoryId
        };
        
        await ctx.Softwares.AddAsync(newSoftware, cancellationToken);
        await ctx.SaveChangesAsync(cancellationToken);
    }
}