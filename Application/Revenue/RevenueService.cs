using System.Text.Json;
using Application.Revenue.DTOs;
using Application.Shared;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Revenue;

public class RevenueService(DatabaseContext ctx, HttpClient httpClient) : IRevenueService
{
    public async Task<RevenueDto> GetTotalRevenueAsync(string? currencyCode, CancellationToken cancellationToken)
    {
        var currentContractRevenue = await ctx.Contracts
            .Where(c => c.Status.StatusName == "Signed")
            .SumAsync(c => c.Price, cancellationToken);
        
        var currentSubscriptionRevenue = await ctx.SubscriptionPayments
            .SumAsync(p => p.Amount, cancellationToken);
        
        var currentRevenue = currentContractRevenue + currentSubscriptionRevenue;

        var predictedContractRevenue = await ctx.Contracts
            .Where(c => c.Status.StatusName == "Signed" || c.Status.StatusName == "Active")
            .SumAsync(c => c.Price, cancellationToken);

        var predictedSubscriptionRevenue = currentSubscriptionRevenue + await ctx.Subscriptions
            .Where(s => s.ActiveUntil >= DateTime.Now)
            .SumAsync(s => s.Price, cancellationToken);
        
        var predictedRevenue = predictedContractRevenue + predictedSubscriptionRevenue;

        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.ToUpper() == "PLN")
        {
            return new RevenueDto(currentRevenue, predictedRevenue, "PLN");
        }
        
        var exchangeRate = await GetExchangeRateAsync(currencyCode, cancellationToken);
        
        return new  RevenueDto(
            Math.Round(currentRevenue/exchangeRate, 2),
            Math.Round(predictedRevenue/exchangeRate, 2), 
            currencyCode.ToUpper()
            );
    }

    public async Task<RevenueDto> GetSoftwareRevenueAsync(int softwareId, string? currencyCode, CancellationToken cancellationToken)
    {
        var currentContractRevenue = await ctx.Contracts
            .Where(c => c.SoftwareId == softwareId && c.Status.StatusName == "Signed")
            .SumAsync(c => c.Price, cancellationToken);

        var currentSubscriptionRevenue = await ctx.SubscriptionPayments
            .Where(p => p.Subscription.SoftwareId == softwareId)
            .SumAsync(p => p.Amount, cancellationToken);

        var currentRevenue = currentContractRevenue + currentSubscriptionRevenue;

        var predictedContractRevenue = await ctx.Contracts
            .Where(c => c.SoftwareId == softwareId && (c.Status.StatusName == "Signed" || c.Status.StatusName == "Active"))
            .SumAsync(c => c.Price, cancellationToken);

        var predictedSubscriptionRevenue = currentSubscriptionRevenue + await ctx.Subscriptions
            .Where(s => s.SoftwareId == softwareId && s.ActiveUntil >= DateTime.Now)
            .SumAsync(s => s.Price, cancellationToken);
        
        var predictedRevenue = predictedContractRevenue + predictedSubscriptionRevenue;
        
        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.ToUpper() == "PLN")
        {
            return new RevenueDto(currentRevenue, predictedRevenue, "PLN");
        }
        
        var exchangeRate = await GetExchangeRateAsync(currencyCode, cancellationToken);

        return new RevenueDto(
            Math.Round(currentRevenue / exchangeRate, 2),
            Math.Round(predictedRevenue / exchangeRate, 2),
            currencyCode.ToUpper()
        );
    }

    private async Task<decimal> GetExchangeRateAsync(string currencyCode, CancellationToken cancellationToken)
    {
        try
        {
            var url = $"http://api.nbp.pl/api/exchangerates/rates/a/{currencyCode.ToLower()}/?format=json";
            var response = await httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new ConflictException($"Currency {currencyCode} is not available");
            }

            var data = await response.Content.ReadAsStringAsync(cancellationToken);
            var nbpData =
                JsonSerializer.Deserialize<NbpDto>(data,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (nbpData?.Rates == null || nbpData.Rates.Count == 0)
            {
                throw new Exception("Failed to get exchange rates");
            }

            return nbpData.Rates[0].Mid;
        } catch(Exception e)
        {
            throw new Exception("Failed to get exchange rates", e);
        }
    }
}