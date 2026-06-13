namespace Application.DTOs;

public record SignUpResponse(
        string AccessToken,
        string RefreshToken
    );