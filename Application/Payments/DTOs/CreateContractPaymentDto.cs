using System.ComponentModel.DataAnnotations;

namespace Application.Payments.DTOs;

public record CreateContractPaymentDto(
    [Required] int ClientId,
    [Required] int ContractId,
    [Required] decimal Amount
    );