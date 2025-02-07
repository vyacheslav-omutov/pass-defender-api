namespace PassDefender.Host.Requests.Login;

public class LoginTwoFactorConfirmRequest
{
    public required string Email { get; set; }
    public required string Code { get; set; }
}