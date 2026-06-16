using Application.DTOs;

namespace Application.Authentication;

public interface IAuthService
{
    public Task<SignInResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken);
    public Task<SignUpResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken);
    public Task<SignInResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
    public Task SignOutAsync(string refreshToken, CancellationToken cancellationToken);
}