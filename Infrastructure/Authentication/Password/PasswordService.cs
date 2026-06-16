using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication.Password;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<object> _hasher = new();

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(null, password);
    }

    public bool VerifyHashedPassword(string hashedPassword, string password)
    {
        return _hasher.VerifyHashedPassword(null, hashedPassword, password) != PasswordVerificationResult.Failed;
    }
}