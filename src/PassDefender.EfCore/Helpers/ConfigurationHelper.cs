using Microsoft.Extensions.Configuration;

namespace PassDefender.EfCore.Helpers;

public static class ConfigurationHelper
{
    internal static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PassDefender.Host/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}