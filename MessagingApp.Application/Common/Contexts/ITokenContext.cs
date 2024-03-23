using MessagingApp.Application.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable ReturnTypeCanBeEnumerable.Global

namespace MessagingApp.Application.Common.Contexts;

public interface ITokenContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    public DbSet<RefreshToken> RefreshTokens { get; }
}