using Calabonga.AspNetCore.AppDefinitions;
using PassDefender.Host.Configs;

namespace PassDefender.Host.Definitions;

public class ConfigDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("smtp"));
    }
}