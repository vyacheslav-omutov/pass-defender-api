namespace PassDefender.Host.Requests.Login;

public class LoginPasswordRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}