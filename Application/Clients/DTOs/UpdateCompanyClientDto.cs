using System.ComponentModel.DataAnnotations;

namespace Application.Clients.DTOs;

public record UpdateCompanyClientDto(
    [Required] string Address,
    [Required] string Email,
    [Required] string PhoneNumber,
    [Required] string Name
    );