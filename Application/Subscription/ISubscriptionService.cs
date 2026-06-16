using Application.Subscription.DTOs;

namespace Application.Subscription;

public interface ISubscriptionService
{
    public Task CreateSubscriptionAsync(CreateSubscriptionDto dto, CancellationToken cancellationToken);
}