using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PassDefender.Domain;
using PassDefender.EfCore;

namespace PassDefender.Host.Definitions;

public class IdentityDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();
        builder.Services.AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddRoleManager<RoleManager<ApplicationRole>>()
            .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
            .AddDefaultTokenProviders();
        
        builder.Services
            .AddIdentityApiEndpoints<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }
}