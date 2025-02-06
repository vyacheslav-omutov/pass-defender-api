using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PassDefender.Domain;
using PassDefender.EfCore.Helpers;

namespace PassDefender.EfCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{ }

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = ConfigurationHelper.BuildConfiguration();
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        return new ApplicationDbContext(builder.Options);
    }
}