using EventDriven.OrderProcessing.Application.Auth.Interfaces;
using EventDriven.OrderProcessing.Application.Common.Interfaces;
using EventDriven.OrderProcessing.Domain.Exceptions;
using EventDriven.OrderProcessing.Domain.Users;
using MediatR;

namespace EventDriven.OrderProcessing.Application.Auth.Commands.Login;
public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwt;
    private readonly IPasswordHasher _hasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwt,
        IPasswordHasher hasher,
        IRefreshTokenRepository refreshTokenRepository
    )
    {
        _userRepository = userRepository;
        _jwt = jwt;
        _hasher = hasher;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginResponse> Handle(
        LoginCommand request, 
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null || !_hasher.Verify(request.Password, user.PasswordHash))
        {
            throw new GenericDomainException("InvalidCredentials", "Email or password is incorrect.");
        }

        var accessToken = _jwt.GenerateToken(user);
        var refreshTokenValue = _jwt.GenerateRefreshToken();

        var refreshToken = new RefreshToken(
            user.Id,
            refreshTokenValue,
            DateTime.UtcNow.AddDays(3)
        );

        await _refreshTokenRepository.AddAsync(refreshToken);

        return new LoginResponse(accessToken, refreshTokenValue);
    }
}
