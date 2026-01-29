using EventDriven.OrderProcessing.Domain.Users;

namespace EventDriven.OrderProcessing.Application.Common.Interfaces;
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
}
