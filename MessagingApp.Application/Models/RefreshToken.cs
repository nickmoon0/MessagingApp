using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;

namespace MessagingApp.Application.Models;

public class RefreshToken : IPersistedObject
{
    public Guid Id { get; private set; }
    public bool Active { get; private set; }

    public User? Owner { get; private set; }
    public string? Token { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    
    private RefreshToken() {}

    private RefreshToken(User owner, string token, DateTime expiryDate)
    {
        Owner = owner;
        Token = token;
        ExpiryDate = expiryDate;
        Active = true;
    }

    public static RefreshToken CreateRefreshToken(User owner, string token, DateTime expiryDate) => 
        new RefreshToken(owner, token, expiryDate);

    // Could make Active set property public but this keeps coding style consistent
    public void InactivateToken() => Active = false;
}