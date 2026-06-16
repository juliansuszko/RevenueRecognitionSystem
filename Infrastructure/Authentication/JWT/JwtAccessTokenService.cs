using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication.JWT;

public class JwtAccessTokenService(IOptions<JwtOptions> options) : IAccessTokenService
{
    private readonly JwtOptions _jwtOptions = options.Value;
    
    public string GenerateAccessToken(AccessTokenDescriptor accessTokenDescriptor)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, accessTokenDescriptor.UserId),
            new Claim(ClaimTypes.Name, accessTokenDescriptor.Login),
            new Claim(ClaimTypes.Role, accessTokenDescriptor.Role)
        };
        
        var symmetricSecurityKey = _jwtOptions.SymmetricSecurityKey ?? throw new ArgumentException("SymmetricSecurityKey is null");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingCredentials,
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationInMinutes)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(securityTokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var random = new byte[96];
        using var gen = RandomNumberGenerator.Create();
        gen.GetBytes(random);
        return Convert.ToBase64String(random);
    }
}