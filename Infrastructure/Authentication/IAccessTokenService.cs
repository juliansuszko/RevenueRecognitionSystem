namespace Infrastructure.Authentication;

public interface IAccessTokenService
{
    public string GenerateAccessToken(AccessTokenDescriptor accessTokenDescriptor);
    public string GenerateRefreshToken();
}