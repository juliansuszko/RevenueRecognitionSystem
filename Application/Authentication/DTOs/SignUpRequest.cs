using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record SignUpRequest
{
    [Required, MaxLength(256)] 
    public string Login { get; set; } = string.Empty;
    
    [Required, MaxLength(256)] 
    public string Password { get; set; } = string.Empty;
    
    [Required, MaxLength(256), Compare(nameof(Password))] 
    public string RepeatPassword { get; set; } = string.Empty;
}