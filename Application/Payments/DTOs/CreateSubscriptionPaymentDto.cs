using System.ComponentModel.DataAnnotations;

namespace Application.Payments.DTOs;

public record CreateSubscriptionPaymentDto(
    [Required] int ClientId,
    [Required] int SubscriptionId,
    [Required] decimal Amount
    );