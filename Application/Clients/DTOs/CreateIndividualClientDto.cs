using System.ComponentModel.DataAnnotations;

namespace Application.Clients.DTOs;

public record CreateIndividualClientDto(
    [Required, MaxLength(200)]
    string Address,
    
    [Required, MaxLength(50)]
    string Email,
    
    [Required, MinLength(9), MaxLength(12)]
    string PhoneNumber,
    
    [Required, MaxLength(50)]
    string FirstName,
    
    [Required, MaxLength(50)]
    string LastName,
    
    [Required, MinLength(11), MaxLength(11)]
    string Pesel
    );