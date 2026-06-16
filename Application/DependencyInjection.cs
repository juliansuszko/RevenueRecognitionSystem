using Application.Authentication;
using Application.Clients;
using Application.Contracts;
using Application.Payments;
using Application.Revenue;
using Application.Software;
using Application.Subscription;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHttpClient();
        
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ISoftwareService, SoftwareService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IRevenueService, RevenueService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IContractService, ContractService>();
        return services;
    }
}
