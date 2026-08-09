namespace SplitWise.Application.DTOs.Auth;

public class RegisterResponse
{
    public int Id { get; set; } 
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}