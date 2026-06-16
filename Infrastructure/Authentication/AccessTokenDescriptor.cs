namespace Infrastructure.Authentication;

public record AccessTokenDescriptor(
    string UserId,
    string Login,
    string Role    
    );