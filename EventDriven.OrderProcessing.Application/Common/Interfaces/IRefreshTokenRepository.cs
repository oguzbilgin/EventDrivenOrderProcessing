using EventDriven.OrderProcessing.Domain.Users;

namespace EventDriven.OrderProcessing.Application.Common.Interfaces;
public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetAsync(string token);
    Task SaveChangesAsync();
}
