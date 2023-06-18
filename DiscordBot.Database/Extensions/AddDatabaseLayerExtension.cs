using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Database.Extensions;

public static class AddDatabaseLayerExtension
{
    public static IServiceCollection AddDatabaseLayer(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Database:ConnectionStrings:Default");
        
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        serviceCollection.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion, 
                server => server.EnableRetryOnFailure(5));
        });

        serviceCollection.AddUnitOfWork<ApplicationDbContext>();

        return serviceCollection;
    }
}