namespace Application.DTOs;

public record SignInResponse(
    string AccessToken,
    string RefreshToken
    );