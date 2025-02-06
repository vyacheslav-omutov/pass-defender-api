using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PassDefender.EfCore.Helpers;

namespace PassDefender.EfCore;

public class DataProtectionDbContext(DbContextOptions<DataProtectionDbContext> options)
    : DbContext(options), IDataProtectionKeyContext
{
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    
    public class DataProtectionDbContextFactory : IDesignTimeDbContextFactory<DataProtectionDbContext>
    {
        public DataProtectionDbContext CreateDbContext(string[] args)
        {
            var configuration = ConfigurationHelper.BuildConfiguration();
            var builder = new DbContextOptionsBuilder<DataProtectionDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DataProtection"));
            return new DataProtectionDbContext(builder.Options);
        }
    }
}