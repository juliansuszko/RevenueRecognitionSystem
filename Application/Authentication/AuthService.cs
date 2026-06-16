using Application.DTOs;
using Application.Shared;
using Domain.Entities;
using Infrastructure.Authentication;
using Infrastructure.Authentication.Password;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Authentication;

public class AuthService(DatabaseContext ctx, IPasswordService passwordService, IAccessTokenService accessTokenService) : IAuthService
{
    public async Task<SignInResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken)
    {
        var user = await ctx.Users.Include(e => e.UserRole).FirstOrDefaultAsync(e =>
                e.Login == request.Login.ToLower(), cancellationToken
        );

        if (user == null)
        {
            throw new UnauthorizedException("User with that login or password doesnt exist");
        }

        if (!passwordService.VerifyHashedPassword(user.PasswordHash, request.Password))
        {
            throw new UnauthorizedException("User with that login or password doesnt exist");
        }

        var accessToken = accessTokenService.GenerateAccessToken(new AccessTokenDescriptor(
            user.UserId.ToString(),
            user.Login,
            user.UserRole.Name
        ));
        var refreshToken = accessTokenService.GenerateRefreshToken();
        
        var token = await ctx.Tokens.FirstOrDefaultAsync(e => e.UserId == user.UserId, cancellationToken);
        if (token == null)
        {
            token ??= new Token
            {
                UserId = user.UserId,
                RefreshToken = refreshToken,
                ExpirationDate = DateTime.Now.AddDays(7)
            };
            await ctx.Tokens.AddAsync(token, cancellationToken);
        }
        else
        {
            token.RefreshToken = refreshToken;
            token.ExpirationDate = DateTime.Now.AddHours(2);
        }
        
        await ctx.SaveChangesAsync(cancellationToken);
        return new SignInResponse(accessToken, refreshToken);
    }

    public async Task<SignUpResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken)
    {
        if (await ctx.Users.AnyAsync(e => e.Login == request.Login, cancellationToken))
        {
            throw new UnauthorizedException("User with that login already exists");
        }
        
        var userRole = await ctx.UserRoles.FirstOrDefaultAsync(e => e.Name == "User", cancellationToken);
        if (userRole == null)
        {
            throw new UnauthorizedException("Role User doesn't exist");
        }

        var user = new User
        {
            Login = request.Login,
            PasswordHash = passwordService.HashPassword(request.Password),
            UserRole = userRole
        };
        
        await ctx.Users.AddAsync(user, cancellationToken);

        var accessToken = accessTokenService.GenerateAccessToken(new AccessTokenDescriptor(
            user.UserId.ToString(),
            user.Login,
            user.UserRole.Name
        ));
        
        var refreshToken = accessTokenService.GenerateRefreshToken();

        var token = new Token
        {
            UserId = user.UserId,
            RefreshToken = refreshToken,
            ExpirationDate = DateTime.Now.AddHours(2)
        };
        
        await ctx.Tokens.AddAsync(token, cancellationToken);
        await ctx.SaveChangesAsync(cancellationToken);
        
        return new SignUpResponse(accessToken, refreshToken);
    }

    public async Task<SignInResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = await ctx.Users
            .Include(e => e.UserRole)
            .Where(e => e.Token.RefreshToken == refreshToken && e.Token.ExpirationDate >= DateTime.Now)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedException("Invalid refresh token");
        }

        var accessToken = accessTokenService.GenerateAccessToken(new AccessTokenDescriptor(
            user.UserId.ToString(),
            user.Login,
            user.UserRole.Name));
        
        var newRefreshToken = accessTokenService.GenerateRefreshToken();
        
        var token = await ctx.Tokens.FirstOrDefaultAsync(e => e.UserId == user.UserId, cancellationToken);
        if (token == null)
        {
            token ??= new Token
            {
                UserId = user.UserId,
                RefreshToken = newRefreshToken,
                ExpirationDate = DateTime.Now.AddDays(7)
            };
            await ctx.Tokens.AddAsync(token, cancellationToken);
        }
        else
        {
            token.RefreshToken = newRefreshToken;
            token.ExpirationDate = DateTime.Now.AddHours(2);
        }
        await ctx.SaveChangesAsync(cancellationToken);
        return new SignInResponse(accessToken, newRefreshToken);
    }

    public async Task SignOutAsync(string refreshToken, CancellationToken cancellationToken)
    {
        await ctx.Tokens
            .Where(e => e.RefreshToken == refreshToken)
            .ExecuteDeleteAsync(cancellationToken);
    }
}