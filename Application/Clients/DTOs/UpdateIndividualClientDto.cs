using System.ComponentModel.DataAnnotations;

namespace Application.Clients.DTOs;

public record UpdateIndividualClientDto(
    [Required] string Address,
    [Required] string Email,
    [Required] string PhoneNumber,
    [Required] string FirstName,
    [Required] string LastName
    );