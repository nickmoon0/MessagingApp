using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;

namespace MessagingApp.Application.Models;

public class RefreshToken : IPersistedObject
{
    public Guid Id { get; set; }
    public bool Active { get; set; }

    public User? Owner { get; set; }
    public string? Token { get; set; }
    public DateTime ExpiryDate { get; set; }
    
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
}