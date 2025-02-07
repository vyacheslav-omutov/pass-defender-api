using Calabonga.AspNetCore.AppDefinitions;
using PassDefender.Host.Services.Email;

namespace PassDefender.Host.Definitions;

public class ServiceDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEmailService, EmailService>();
    }
}