using System.ComponentModel.DataAnnotations;

namespace Application.Subscription.DTOs;

public record CreateSubscriptionDto(
    [Required]
    string Name,
    [Required, Range(1, 24)]
    int PeriodInMonths,
    [Required]
    int SoftwareId,
    [Required]
    int ClientId
    );