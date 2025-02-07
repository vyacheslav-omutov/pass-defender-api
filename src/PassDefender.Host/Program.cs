using System.Text.Json.Serialization;
using Calabonga.AspNetCore.AppDefinitions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.AddDefinitions(typeof(Program));

    builder.Services.AddSerilog();
    builder.Services.AddAntiforgery();
    builder.Services.AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();
    
    app.UseDefinitions();
    app.UseSerilogRequestLogging();
    
    app.UseCors(policyBuilder => policyBuilder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
    );
    
    app.UseHttpsRedirection();
    
    app.MapGet("/", () => Results.LocalRedirect("/swagger"))
        .ExcludeFromDescription();
    
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex, "Unhandled exception");
}
finally
{
    await Log.CloseAndFlushAsync();
}