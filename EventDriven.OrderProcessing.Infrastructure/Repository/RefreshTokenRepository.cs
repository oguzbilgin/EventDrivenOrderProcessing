using EventDriven.OrderProcessing.Application.Common.Interfaces;
using EventDriven.OrderProcessing.Domain.Users;
using EventDriven.OrderProcessing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventDriven.OrderProcessing.Infrastructure.Repository;
public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken token)
    {
        _context.Add(token);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetAsync(string token)
    {
        return await _context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
