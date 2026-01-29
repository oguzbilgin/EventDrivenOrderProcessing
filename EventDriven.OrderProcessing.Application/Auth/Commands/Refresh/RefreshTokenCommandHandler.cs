using EventDriven.OrderProcessing.Application.Auth.Commands.Login;
using EventDriven.OrderProcessing.Application.Auth.Interfaces;
using EventDriven.OrderProcessing.Application.Common.Interfaces;
using EventDriven.OrderProcessing.Domain.Exceptions;
using EventDriven.OrderProcessing.Domain.Users;
using MediatR;

namespace EventDriven.OrderProcessing.Application.Auth.Commands.Refresh;
public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwt;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IJwtTokenGenerator jwt
        )
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwt = jwt;
    }
    public async Task<LoginResponse> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var token = await _refreshTokenRepository.GetAsync(request.RefreshToken);

        if (token is null || token.IsExpired || token.IsRevoked)
            throw new InvalidOperationException("Invalid refresh token");

        token.Revoke();

        var user = await _userRepository.GetByIdAsync(token.UserId);

        if (user is null)
        {
            throw new Exception("User not found");
        }

        var newAccessToken = _jwt.GenerateToken(user);
        var newRefreshToken = _jwt.GenerateRefreshToken();

        await _refreshTokenRepository.AddAsync(
            new RefreshToken(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7))
        );

        await _refreshTokenRepository.SaveChangesAsync();

        return new LoginResponse(newAccessToken, newRefreshToken);
    }
}
