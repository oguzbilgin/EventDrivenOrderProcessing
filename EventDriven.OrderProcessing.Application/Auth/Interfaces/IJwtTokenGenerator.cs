using EventDriven.OrderProcessing.Domain.Users;

namespace EventDriven.OrderProcessing.Application.Auth.Interfaces;
public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}
