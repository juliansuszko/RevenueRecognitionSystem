using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record SignInRequest(
    [Required, MaxLength(256)] string Login, 
    [Required, MaxLength(256)] string Password
);