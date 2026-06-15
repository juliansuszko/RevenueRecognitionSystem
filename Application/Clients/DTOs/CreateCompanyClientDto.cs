using System.ComponentModel.DataAnnotations;

namespace Application.Clients.DTOs;

public record CreateCompanyClientDto(
    [Required, MaxLength(200)]
    string Address,
    
    [Required, MaxLength(50)]
    string Email,
    
    [Required, MinLength(9), MaxLength(12)]
    string PhoneNumber,
    
    [Required, MaxLength(150)]
    string Name,
    
    [Required, MinLength(10), MaxLength(10)]
    string Krs
    );