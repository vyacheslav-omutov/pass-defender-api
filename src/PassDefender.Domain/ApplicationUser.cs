using Microsoft.AspNetCore.Identity;

namespace PassDefender.Domain;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? AccessTokenExpiry { get; set; }
}