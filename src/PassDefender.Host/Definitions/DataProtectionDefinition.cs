using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using PassDefender.EfCore;

namespace PassDefender.Host.Definitions;

public class DataProtectionDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        var authenticatedEncryptorConfiguration = new AuthenticatedEncryptorConfiguration()
        {
            EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM
        };
        
        builder.Services.AddDataProtection()
            .SetApplicationName("PassDefender")
            .SetDefaultKeyLifetime(TimeSpan.FromDays(14))
            .PersistKeysToDbContext<DataProtectionDbContext>()
            .UseCryptographicAlgorithms(authenticatedEncryptorConfiguration);
    }
}