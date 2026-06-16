using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs;

public record CreateContractDto(
    [Required] int ClientId,
    [Required] int SoftwareId,
    [Required] DateTime EndDate,
    [Required] 
    [Range(1, 4)]
    int YearsOfSupport
    );