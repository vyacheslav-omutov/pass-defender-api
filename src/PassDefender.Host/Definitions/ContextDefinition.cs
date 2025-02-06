using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.EntityFrameworkCore;
using PassDefender.EfCore;

namespace PassDefender.Host.Definitions;

public class ContextDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
        
        builder.Services.AddDbContext<DataProtectionDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DataProtectionDb")));
    }
}