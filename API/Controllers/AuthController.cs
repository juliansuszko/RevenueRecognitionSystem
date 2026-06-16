using Application.Authentication;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest signInRequest, CancellationToken cancellationToken)
    {
        var result = await authService.SignInAsync(signInRequest, cancellationToken);
        AppendRefreshTokenCookie(HttpContext, result.RefreshToken);

        return Ok(new {accessToken = result.AccessToken});
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest signUpRequest, CancellationToken cancellationToken)
    {
        var result = await authService.SignUpAsync(signUpRequest, cancellationToken);
        AppendRefreshTokenCookie(HttpContext, result.RefreshToken);

        return Ok(new {accessToken = result.AccessToken});
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        var refreshToken = GetRefreshTokenFromCookie(HttpContext);
        if (refreshToken is null)
        {
            return Unauthorized();
        }
        
        var result = await authService.RefreshAsync(refreshToken, cancellationToken);
        
        AppendRefreshTokenCookie(HttpContext, result.RefreshToken);
        return Ok(new {accessToken = result.AccessToken});
    }

    [HttpPost("sign-out")]
    public async Task<IActionResult> SignOut(CancellationToken cancellationToken)
    {
        var refreshToken = GetRefreshTokenFromCookie(HttpContext);
        if (refreshToken is null)
        {
            return NoContent();
        }
        
        await authService.SignOutAsync(refreshToken, cancellationToken);
        
        RemoveRefreshTokenCookie(HttpContext);
        return NoContent();
    }
    
    private static void AppendRefreshTokenCookie(HttpContext httpContext, string refreshToken)
    {
        httpContext.Response.Cookies.Append("ref-token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
    }

    private static string? GetRefreshTokenFromCookie(HttpContext ctx)
    {
        return ctx.Request.Cookies["ref-token"];
    }
    
    private static void RemoveRefreshTokenCookie(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete("ref-token", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
    }
}