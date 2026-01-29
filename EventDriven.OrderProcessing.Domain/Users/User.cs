using EventDriven.OrderProcessing.Domain.Common;

namespace EventDriven.OrderProcessing.Domain.Users;
public sealed class User : BaseEntity
{
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string Role { get; private set; } = "User";

    private User() { } // EF

    public User(string email, string passwordHash, string role)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
}
