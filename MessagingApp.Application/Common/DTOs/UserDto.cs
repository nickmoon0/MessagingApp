namespace MessagingApp.Application.Common.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}