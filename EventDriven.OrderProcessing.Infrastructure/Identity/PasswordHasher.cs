using EventDriven.OrderProcessing.Application.Auth.Interfaces;

namespace EventDriven.OrderProcessing.Infrastructure.Identity;
public sealed class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string hash)
        => BCrypt.Net.BCrypt.Verify(password, hash);
}
