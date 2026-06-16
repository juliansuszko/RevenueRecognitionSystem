using Application.Revenue.DTOs;

namespace Application.Revenue;

public interface IRevenueService
{
    Task<RevenueDto> GetTotalRevenueAsync(string? currencyCode, CancellationToken cancellationToken);
    Task<RevenueDto> GetSoftwareRevenueAsync(int softwareId, string? currencyCode, CancellationToken cancellationToken);
}