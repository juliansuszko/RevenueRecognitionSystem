using System.ComponentModel.DataAnnotations;

namespace Application.Software.DTOs;

public record CreateSoftwareDto(
    [Required, MaxLength(150)]
    string Name,
    [Required, MaxLength(500)]
    string Description,
    [Required, MaxLength(50)]
    string LatestVersion,
    [Required]
    int CategoryId
    );