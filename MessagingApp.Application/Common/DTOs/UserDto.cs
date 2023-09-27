namespace MessagingApp.Application.Common.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

    public UserDto() {}
    
    public UserDto(string? username, string? password)
    {
        Username = username;
        Password = password;
    }
    
    
}