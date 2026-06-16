using Application.Payments.DTOs;

namespace Application.Payments;

public interface IPaymentService
{
    Task CreateContractPaymentAsync(CreateContractPaymentDto dto, CancellationToken cancellationToken);
    Task CreateSubscriptionPaymentAsync(CreateSubscriptionPaymentDto dto, CancellationToken cancellationToken);
}