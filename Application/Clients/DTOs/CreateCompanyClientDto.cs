using System.ComponentModel.DataAnnotations;

namespace Application.Clients.DTOs;

public record CreateCompanyClientDto(
    [Required, MaxLength(200)]
    string Address,
    
    [Required, MaxLength(50)]
    string Email,
    
    [Required, MaxLength(15)]
    string PhoneNumber,
    
    [Required, MaxLength(150)]
    string Name,
    
    [Required, MaxLength(10)]
    string Krs
    );