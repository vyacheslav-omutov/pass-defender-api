namespace PassDefender.Host.Configs;

public class SmtpConfig
{
    public string Name { get; init; } = default!;
    public string From { get; init; } = default!;
    public string Host { get; init; } = default!;
    public int Port { get; init; } = 465;
    public bool UseSsl { get; init; } = true;
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}