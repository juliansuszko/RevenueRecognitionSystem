namespace Infrastructure.Authentication.Password;

public interface IPasswordService
{
    public string HashPassword(string password);
    public bool VerifyHashedPassword(string hashedPassword, string password);
}