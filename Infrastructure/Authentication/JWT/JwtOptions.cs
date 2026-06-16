namespace Infrastructure.Authentication.JWT;

public class JwtOptions
{
    public string SymmetricSecurityKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int AccessTokenExpirationInMinutes {get; set;}
}